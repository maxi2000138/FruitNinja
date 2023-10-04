using UnityEngine;

public class ShootApplier : MonoBehaviour, IMover
{
    [SerializeField] private float _shootSpeed;

    private Vector2 _shootVector = Vector2.zero;
    
    public void Shoot(Vector2 shootVector)
    {
        _shootVector = shootVector * _shootSpeed;
    }

    public Vector2 Move(float fixedDeltaTime, Vector2 lastMovementVector)
    {
        return _shootVector * fixedDeltaTime;
    }
}
