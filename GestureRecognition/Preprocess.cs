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
using System.Xml.Serialization;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torchvision;
using static TorchSharp.torch.nn.functional;
using OpenCV.Net;

namespace GestureRecognition;

[Combinator]
[WorkflowElementCategory(ElementCategory.Transform)]
public class Preprocess
{
    [XmlIgnore]
    public Device Device { get; set; }

    private int _targetSize = 640;

    public IObservable<Tensor> Process(IObservable<IplImage> source)
    {
        return source.Select(image =>
        {
            if (image is null)
                return null;

            var device = Device ?? CPU;

            var tensor = OpenCVHelper.ToTensor(image, device)
                .to(device)
                .permute(2, 0, 1)
                .unsqueeze(0);

            if (tensor.NumberOfElements == 0)
                return null;

            var channels = tensor.size(1);
            var height = tensor.size(2);
            var width = tensor.size(3);

            // Scale image symmetrically such that the largest dimension matches target size
            var scale = (float)_targetSize / Math.Max(height, width);
            var newHeight = Math.Max(1, (int)(height * scale));
            var newWidth = Math.Max(1, (int)(width * scale));

            tensor = interpolate(tensor, [newHeight, newWidth], mode: InterpolationMode.Bilinear, align_corners: false);

            // Pad to size of _targetSize x _targetSize
            var padWidth = _targetSize - newWidth;
            var padHeight = _targetSize - newHeight;
            var padLeft = padWidth / 2;
            var padRight = padWidth - padLeft;
            var padTop = padHeight / 2;
            var padBottom = padHeight - padTop;

            tensor = pad(tensor, [padLeft, padRight, padTop, padBottom], PaddingModes.Constant, 0);

            return tensor.to_type(ScalarType.Float32) / 255.0f;
        });
    }
}
