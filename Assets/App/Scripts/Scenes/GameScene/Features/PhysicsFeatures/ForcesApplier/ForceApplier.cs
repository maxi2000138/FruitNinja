using UnityEngine;

public class ForceApplier : MonoBehaviour, IMover
{
    private Vector2 _forceVector;

    public void Shoot(Vector2 forceVector)
    {
        _forceVector = forceVector;
    }

    public Vector2 Move(float deltaTime)
    {
        return _forceVector * deltaTime;
    }
}
