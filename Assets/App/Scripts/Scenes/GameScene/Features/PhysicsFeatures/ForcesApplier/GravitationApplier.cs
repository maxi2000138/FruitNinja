using UnityEngine;

public class GravitationApplier : MonoBehaviour, IMover
{
    [SerializeField] private GravitationConfig _gravitationConfig;
    private float _velocity = 0f;
    
    public Vector2 Move(float fixedDeltaTime)
    {
        _velocity += _gravitationConfig.StartGravityValue * fixedDeltaTime;
        float deltaY = _velocity * fixedDeltaTime;
        return new Vector3(0f, deltaY, 0f);
    }
}