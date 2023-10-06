using UnityEngine;

public class ScreenSettingsProvider : MonoBehaviour, IScreenSettingsProvider
{
    public Vector2 CameraStartPoint => _camera.ScreenToWorldPoint(Vector2.zero);
    public float CameraHeight => _camera.orthographicSize * 2;
    public float CameraWidth => CameraHeight * _camera.aspect;
    
    [SerializeField] private Camera _camera;
    
    
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
