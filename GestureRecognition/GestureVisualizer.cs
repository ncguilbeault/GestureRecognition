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
using Bonsai.Design;
using OpenCV.Net;

[assembly: TypeVisualizer(typeof(GestureRecognition.GestureVisualizer),
    Target = typeof(List<GestureRecognition.GestureDataFrame>))]

namespace GestureRecognition;

public class GestureVisualizer : DialogTypeVisualizer
{
    /// <inheritdoc/>
    public override void Show(object value)
    {
    }
        
    /// <inheritdoc/>
    public override void Load(IServiceProvider provider)
    {
    }

    /// <inheritdoc/>
    public override void Unload()
    {
    }
}
