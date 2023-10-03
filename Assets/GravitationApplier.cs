using UnityEngine;

public class GravitationApplier : MonoBehaviour, IMover
{
    [SerializeField] private GameConfig _gameConfig;
    private float _time;
    private float _velocity;

    private void OnEnable()
    {
        _time = 0;
        _velocity = 0;
    }

    public Vector2 Move(float fixedDeltaTime)
    {
        _time += fixedDeltaTime;
        _velocity = _gameConfig.GravitationalConstant * (_time * _time) / 2f;
        float deltaY = _velocity * fixedDeltaTime;
        return new Vector3(0f, deltaY, 0f);
    }
}