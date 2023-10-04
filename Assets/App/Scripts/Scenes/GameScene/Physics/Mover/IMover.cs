using UnityEngine;

public interface IMover
{
    public Vector2 Move(float fixedDeltaTime, Vector2 lastMovementVector);
}