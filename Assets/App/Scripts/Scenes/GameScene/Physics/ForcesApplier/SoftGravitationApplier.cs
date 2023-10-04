using UnityEngine;

public class SoftGravitationApplier : MonoBehaviour, IMover
{
    [SerializeField] private GameConfig _gameConfig;
    private float _time;
    private float _velocity;

    private void OnEnable()
    {
        _time = 0;
        _velocity = 0;
    }

    public Vector2 Move(float fixedDeltaTime, Vector2 lastMovementVector)
    {
        _time += fixedDeltaTime;
        float gravConst = _gameConfig.UpGravitationalConstant;

        if (lastMovementVector.y <= _gameConfig.BoundaryVelocity)
            gravConst = _gameConfig.DownGravitationalConstant;

        _velocity += gravConst * fixedDeltaTime;
        float deltaY = _velocity * fixedDeltaTime;
        return new Vector3(0f, deltaY, 0f);
    }
}