# Gesture Recognition
Bonsai example of running a YOLOv10n-based hand gesture recognition model for online inference of hand gestures.

## Overview
- `weights/` holds the pretrained `YOLOv10n_gestures.pt` checkpoint (from the [HaGRID repo](https://github.com/hukenovs/hagrid)) and the exported TorchScript model consumed by **Bonsai.ML.Torch**.
- `generate_torchscript_model.py` loads the raw checkpointed weights into a `YOLO` model using the `ultralytics` package, and then registers all custom Ultralytics/Torch modules required to serialize the model. It then exports the loaded model to TorchScript format and places it into the `weights/` folder.
- `GestureRecognition/` contains a Bonsai package with operators for decoding the models output and overlaying the labels/bounding boxes onto an image.
- `CustomPackages/` contains custom `.nupkg` files to be used by Bonsai. The Bonsai.ML pre-release packages (v0.4.2-rc.1) from the [Bonsai.ML repo](https://github.com/ncguilbeault/machinelearning/releases/tag/v0.4.2-rc.1) should be placed here.
- `demo.bonsai` provides a minimal example workflow running online inference to detect hand gestures.

## Quick Start
1. **Initial Setup**
	- Download the `YOLOv10n_gestures.pt` weights checkpoint from the [HaGRID repo](https://github.com/hukenovs/hagrid) and place it into the `weights/` folder.
	- Download the `Bonsai.ML` pre-release packages (v0.4.2-rc.1) from the [Bonsai.ML releases page](https://github.com/ncguilbeault/machinelearning/releases/tag/v0.4.2-rc.1). Place the `.nupkg` files into the `CustomPackages/` folder.
2. **Python**
    - Ensure uv is installed or Python (v3.12).
	- Install dependencies with `uv sync` (uv is recommended) or `pip install -e .`.
	- Generate the TorchScript scripted model from the model checkpoint: `uv run generate_torchscript_model.py`.
3. **Dotnet**
    - Ensure `dotnet` (v8) is installed.
	- Build and package the .NET library: `dotnet build -c Release && dotnet pack`.
4. **Bonsai**
	- Bootstrap the provided Bonsai environment. On Windows, you can run `bonsai --no-editor` from the command line in the root folder. On Linux, you can run the `dotnet new bonsaienvl` command from the root folder.

## General Usage
- Load the TorchScript model with the `LoadScriptModule` operator.
- Use the `Preprocess` node on the video stream to convert raw images into tensors.
- Pass the processed tensor to the `Forward` operator, making sure the model is loaded and set to the `Model` property.
- Convert the model's output into a `List<GestureDataFrame>` using the `GestureData` operator. 
- Visualize the results.

## Demo
![](./demo.png)

