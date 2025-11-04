from ultralytics import YOLO
from ultralytics.nn.tasks import YOLOv10DetectionModel
from torch.serialization import add_safe_globals
from torch.nn.modules.container import Sequential, ModuleList
from ultralytics.nn.modules.conv import Conv, Concat
from torch.nn.modules.conv import Conv2d
from torch.nn.modules.batchnorm import BatchNorm2d
from torch.nn.modules.activation import SiLU
from torch.nn.modules.linear import Identity
from torch.nn.modules.pooling import MaxPool2d
from torch.nn.modules.upsampling import Upsample
from torch.nn.modules.loss import BCEWithLogitsLoss
from ultralytics.nn.modules.block import C2f, Bottleneck, SCDown, SPPF, PSA, Attention, C2fCIB, CIB, RepVGGDW, DFL
from ultralytics.nn.modules.head import v10Detect
from ultralytics.utils import IterableSimpleNamespace
from ultralytics.utils.loss import v10DetectLoss, v8DetectionLoss, BboxLoss
from ultralytics.utils.tal import TaskAlignedAssigner

import numpy as np
from numpy.core.multiarray import scalar as np_scalar
from numpy import dtype as np_dtype
from numpy.dtypes import Float64DType

add_safe_globals([
    YOLOv10DetectionModel, 
    Sequential,
    ModuleList,
    Conv,
    Conv2d,
    BatchNorm2d,
    SiLU,
    C2f,
    Bottleneck,
    SCDown,
    Identity,
    SPPF,
    MaxPool2d,
    PSA,
    Attention,
    Upsample,
    Concat,
    C2fCIB,
    CIB,
    RepVGGDW,
    v10Detect,
    DFL,
    IterableSimpleNamespace,
    v10DetectLoss,
    v8DetectionLoss,
    BCEWithLogitsLoss,
    TaskAlignedAssigner,
    BboxLoss,
    np_scalar,
    np_dtype,
    Float64DType
])
m = YOLO("weights/YOLOv10n_gestures.pt")
m.export(format="torchscript")