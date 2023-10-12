using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider
{
    public interface IScreenSettingsProvider
    {
        Vector2 ViewportToWorldPosition(Vector2 viewportPosition);
    }
}