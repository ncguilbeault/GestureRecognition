using Bonsai;
using Bonsai.Expressions;
using Bonsai.ML.Torch;
using Bonsai.Design;
using Bonsai.Vision.Design;
using System.Linq.Expressions;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TorchSharp;
using static TorchSharp.torch;
using OpenCV.Net;

[assembly: TypeVisualizer(typeof(GestureRecognition.GestureVisualizerOverlay),
    Target = typeof(MashupSource<ImageMashupVisualizer, GestureRecognition.GestureVisualizer>))]

namespace GestureRecognition;

public class GestureVisualizerOverlay : DialogTypeVisualizer
{
    private ImageMashupVisualizer visualizer;
    
    private int _targetSize = 640;

    /// <inheritdoc/>
    public override void Show(object value)
    {
        var image = visualizer.VisualizerImage;

        if (value is not List<GestureDataFrame> gestureData || gestureData.Count == 0)
            return;

        for (int i = 0; i < gestureData.Count; i++)
        {
            var gestureFrame = gestureData[i];

            var x = gestureFrame.BoundingBox.X;
            var y = gestureFrame.BoundingBox.Y;
            var width = gestureFrame.BoundingBox.Width;
            var height = gestureFrame.BoundingBox.Height;

            if (image.Width == image.Height)
            {
                var scale = (float)image.Width / _targetSize;
                x *= scale;
                y *= scale;
                width *= scale;
                height *= scale;
            }
            else if (image.Width > image.Height)
            {
                var widthScale = (float)image.Width / _targetSize;
                x *= widthScale;
                width *= widthScale;

                var aspectRatio = (float)image.Height / image.Width;
                var targetHeight = _targetSize * aspectRatio;
                var heightScale = image.Height / targetHeight;
                var heightOffset = (_targetSize - targetHeight) / 2;

                y -= heightOffset;
                y *= heightScale;
                height *= heightScale;
            }
            else // image.Height > image.Width
            {
                var heightScale = (float)image.Height / _targetSize;
                y *= heightScale;
                height *= heightScale;

                var aspectRatio = (float)image.Width / image.Height;
                var targetWidth = _targetSize * aspectRatio;
                var widthScale = image.Width / targetWidth;
                var widthOffset = (_targetSize - targetWidth) / 2;

                x -= widthOffset;
                x *= widthScale;
                width *= widthScale;
            }

            var boundingBox = new Rect(
                (int)Math.Max(0, Math.Round(x)),
                (int)Math.Max(0, Math.Round(y)),
                (int)Math.Min(Math.Round(width), image.Width - x),
                (int)Math.Min(Math.Round(height), image.Height - y));

            CV.Rectangle(image, boundingBox, new OpenCV.Net.Scalar(255, 0, 0, 0), 2);
            CV.PutText(image, gestureFrame.Gesture.ToString(), new Point(boundingBox.X, boundingBox.Y - 10), new Font(FontFace.HersheySimplex, 1, 1), new OpenCV.Net.Scalar(0, 255, 0, 0));

        }

    }
        
    /// <inheritdoc/>
    public override void Load(IServiceProvider provider)
    {
        visualizer = (ImageMashupVisualizer)provider.GetService(typeof(MashupVisualizer));
    }

    /// <inheritdoc/>
    public override void Unload()
    {
        // overlay.Dispose();
    }
}
