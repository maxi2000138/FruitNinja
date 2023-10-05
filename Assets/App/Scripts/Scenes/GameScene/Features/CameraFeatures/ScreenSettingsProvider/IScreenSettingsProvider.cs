using UnityEngine;

public interface IScreenSettingsProvider
{
    Vector2 ViewportToWorldPosition(Vector2 viewportPosition);
}