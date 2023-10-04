using UnityEngine;

public class CameraFeaturesProvider
{
    private readonly Camera _camera;
    public float ScreenWidth =>
        _camera.orthographicSize*2;
    public float ScreenHeight =>
        ScreenWidth * _camera.aspect;
    
    public CameraFeaturesProvider(Camera camera)
    {
        _camera = camera;
    }

    public Vector2 ViewportToWorldPosition(Vector2 viewportPosition)
    {
        Vector2 point = _camera.ViewportToWorldPoint(viewportPosition);
        return point;
    }
    
    public Vector2 WorldToViewportPosition(Vector2 worldPosition)
    {
        Vector2 point = _camera.WorldToViewportPoint(worldPosition);
        return point;
    }
}
