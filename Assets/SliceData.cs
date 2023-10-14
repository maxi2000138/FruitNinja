using UnityEngine;

public class SliceData
{
    public readonly Vector2 Position;
    public readonly Vector2 Rotation;
    public readonly Vector2 Scale;

    public SliceData(Vector2 position, Vector2 rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}
