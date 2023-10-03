using UnityEngine;

public class CameraProvider
{
    private Vector2 _cameraStartPoint;
    private float _cameraWidth;
    private float _cameraHeight;
    private readonly Camera _camera;

    public CameraProvider(Camera camera)
    {
        _camera = camera;
    }

    public Vector2 ViewportToWorldPosition(float viewportPositionX, float viewportPositionY)
    {
        return _camera.ViewportToWorldPoint(new Vector3(viewportPositionX, viewportPositionY));
    }
}
