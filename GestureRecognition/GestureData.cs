using Bonsai;
using Bonsai.Expressions;
using Bonsai.ML.Torch;
using Bonsai.Vision.Design;
using System.Linq.Expressions;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torchvision;

namespace GestureRecognition;

[Combinator]
[WorkflowElementCategory(ElementCategory.Transform)]
public class GestureData
{
    public float ConfidenceThreshold { get; set; } = 0.5f;

    public IObservable<List<GestureDataFrame>> Process(IObservable<Tensor> source)
    {
        return source.Select(tensor =>
        {
            if (tensor is null || tensor.NumberOfElements == 0 || tensor.Dimensions != 3 || tensor.size(2) < 6)
                return [];

            var flattenedTensor = tensor.view(-1, tensor.size(2));

            var aboveThreshold = flattenedTensor[torch.TensorIndex.Colon, 4] > ConfidenceThreshold;
            var detections = flattenedTensor[aboveThreshold];

            if (detections.size(0) == 0)
                return [];

            var detectionResults = new List<GestureDataFrame>();

            if (detections.dtype != ScalarType.Float32)
                detections = detections.to(ScalarType.Float32);

            for (var i = 0; i < detections.size(0); i++)
            {
                var x = detections[i, 0].item<float>();
                var y = detections[i, 1].item<float>();
                var width = detections[i, 2].item<float>() - x;
                var height = detections[i, 3].item<float>() - y;

                var boundingBox = new BoundingBox(
                    x, y,
                    width,
                    height
                );

                var confidence = detections[i, 4].item<float>();
                var gestureValue = (int)detections[i, 5].item<float>();

                var gestureDataFrame = new GestureDataFrame(
                    gesture: (Gesture)gestureValue,
                    confidence: confidence,
                    boundingBox: boundingBox
                );

                detectionResults.Add(gestureDataFrame);
            }

            return detectionResults;
        });
    }
}
