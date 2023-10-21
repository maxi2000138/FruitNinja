using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider
{
    public interface IScreenSettingsProvider
    {
        Vector2 ViewportToWorldPosition(Vector2 viewportPosition);
        Vector2 WorldToViewportPosition(Vector2 worldPosition);
        Vector2 ScreenToWorldPosition(Vector2 worldPosition);
        Vector2 WorldToScreenPosition(Vector2 worldPosition);
        Vector2 ViewportToScreenPosition(Vector2 worldPosition);
    }
}