using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider
{
    public class ScreenSettingsProvider : MonoBehaviour, IScreenSettingsProvider
    {
        public Vector2 CameraStartPoint => _camera.ScreenToWorldPoint(Vector2.zero);
        public Vector2 CameraCenter => _camera.transform.position;
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
        
        public Vector2 WorldToScreenPosition(Vector2 worldPosition)
        {
            Vector2 point = _camera.WorldToScreenPoint(worldPosition);
            return point;
        }

    
        public Vector2 ScreenToWorldPosition(Vector2 worldPosition)
        {
            Vector2 point = _camera.ScreenToWorldPoint(worldPosition);
            return point;
        }
        
        public Vector2 ViewportToScreenPosition(Vector2 worldPosition)
        {
            Vector2 point = _camera.ViewportToScreenPoint(worldPosition);
            return point;
        }

    }
}   
