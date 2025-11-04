namespace GestureRecognition;

public readonly struct GestureDataFrame(
    Gesture gesture,
    float confidence,
    BoundingBox boundingBox)
{
    public Gesture Gesture => gesture;
    public float Confidence => confidence;
    public BoundingBox BoundingBox => boundingBox;
}
