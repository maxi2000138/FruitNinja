using UnityEngine;

public class ShootApplier : MonoBehaviour, IMover
{
    private Vector2 _shootVector;

    public void Shoot(Vector2 shootVector)
    {
        _shootVector = shootVector;
    }

    public Vector2 Move(float fixedDeltaTime)
    {
        return _shootVector * fixedDeltaTime;
    }
}
