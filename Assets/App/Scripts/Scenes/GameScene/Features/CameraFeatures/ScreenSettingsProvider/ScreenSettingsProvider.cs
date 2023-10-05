using UnityEngine;

public class ScreenSettingsProvider : IScreenSettingsProvider
{
    private readonly Camera _camera;
    
    public ScreenSettingsProvider(Camera camera)
    {
        _camera = camera;
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition)
    {
        Vector2 point = _camera.ViewportToWorldPoint(viewportPosition);
        return point;
    }
}
