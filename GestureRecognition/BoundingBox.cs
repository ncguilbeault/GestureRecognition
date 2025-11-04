namespace GestureRecognition;

public readonly struct BoundingBox(
    float x,
    float y,
    float width,
    float height)
{
    public float X => x;
    public float Y => y;
    public float Width => width;
    public float Height => height;
}