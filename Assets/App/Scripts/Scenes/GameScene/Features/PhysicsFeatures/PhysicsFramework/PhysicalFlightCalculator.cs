using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

public class PhysicalFlightCalculator : IInitializable
{
    private float _highestYValue;
    private readonly GravitationConfig _gravitationConfig;
    private readonly ScreenSettingsProvider _screenSettingsProvider;


    public PhysicalFlightCalculator(ScreenSettingsProvider screenSettingsProvider, GravitationConfig gravitationConfig)
    {
        _screenSettingsProvider = screenSettingsProvider;
        _gravitationConfig = gravitationConfig;

    }

    public void Initialize()
    {
        _highestYValue = _screenSettingsProvider.ViewportToWorldPosition(new Vector2(0, 1)).y;
    }
    
    public Vector2 ConstrainSpeed(float positionY, Vector2 moveVector)
    {
        float maxHeight = GetPathHeight(positionY);
        float neededYVelocity = GetNeededYVelocityForHeight(maxHeight);
        float coef = moveVector.y / neededYVelocity;
        if (coef > 1)
        {
            moveVector /= coef;
        }

        return moveVector;
    }
    
    
    public float GetFlyTimeFromYPosition(float yPosition) =>
        GetFlyTimeFromVelocity(GetNeededYVelocityForHeight(GetPathHeight(yPosition)));

    private float GetFlyTimeFromVelocity(float yVelocity) => 
        yVelocity * 2 / Mathf.Abs(_gravitationConfig.StartGravityValue);

    private float GetPathHeight(float positionY) => 
        _highestYValue - positionY;

    private float GetNeededYVelocityForHeight(float maxHeight) => 
        Mathf.Sqrt(2 * Mathf.Abs(_gravitationConfig.StartGravityValue * maxHeight));
}
