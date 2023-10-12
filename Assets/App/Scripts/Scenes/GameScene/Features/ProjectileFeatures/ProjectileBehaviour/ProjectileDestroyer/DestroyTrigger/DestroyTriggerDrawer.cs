using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger
{
    public class DestroyTriggerDrawer : MonoBehaviour
    {
        [SerializeField] private ProjectileConfig _projectileConfig;
        [SerializeField] private ScreenSettingsProvider _screenSettingsProvider;
    
        private Vector2 _leftPoint;
        private Vector2 _rightPoint;

        private void OnDrawGizmos()
        {
            _leftPoint = _screenSettingsProvider.ViewportToWorldPosition(new Vector2(0, 0));
            _rightPoint = _screenSettingsProvider.ViewportToWorldPosition(new Vector2(1, 0));
            _leftPoint.y += _projectileConfig.DestroyTriggerOffset;
            _rightPoint.y += _projectileConfig.DestroyTriggerOffset;
            Gizmos.color = Color.black;
            Gizmos.DrawLine(_leftPoint, _rightPoint);
        }
    }
}
