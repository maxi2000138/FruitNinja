using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using UnityEngine;

public class MagnetSuctionApplier : IMover
{
    private readonly ProjectileObject _projectileObject;
    private readonly Vector2 _magnetPosition;
    private readonly BonusesConfig _bonusesConfig;

    public MagnetSuctionApplier(ProjectileObject projectileObject, Vector2 magnetPosition, BonusesConfig bonusesConfig)
    {
        _projectileObject = projectileObject;
        _magnetPosition = magnetPosition;
        _bonusesConfig = bonusesConfig;
    }
    
    public Vector2 Move(Vector2 movementVector, float deltaTime, float timeScale)
    {
        Vector2 directionVector = _magnetPosition - (Vector2)_projectileObject.transform.position;
        float distance = directionVector.magnitude;
        float stoppingDistance = _bonusesConfig.StopingDistance;
        float clampDistance = Mathf.Clamp(distance,0.5f, distance);
        if (clampDistance > stoppingDistance)
        {
            return directionVector.normalized * (_bonusesConfig.ForceVelocity * deltaTime);
        }

        Vector2 clampMovementVector = movementVector;
        
        if(movementVector.magnitude < 0.1f)
            clampMovementVector = Vector2.zero;

        float directionScaleFactor = _bonusesConfig.DirectionScaleFactor;
        
        if (distance < _bonusesConfig.ScaleFactorDistance)
            directionScaleFactor *= _bonusesConfig.scaleLitleFactor;

        return -clampMovementVector * _bonusesConfig.ScaleFactor / clampDistance + directionVector / clampDistance * directionScaleFactor;
    }

    public void Clear()
    {
        
    }
}
