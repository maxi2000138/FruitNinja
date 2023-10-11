using UnityEngine;

public class GravitationApplier : MonoBehaviour, IMover
{
    [SerializeField] private GravitationConfig _gravitationConfig;
    private float _velocity = 0f;
    
    public Vector2 Move(float deltaTime)
    {
        _velocity += _gravitationConfig.StartGravityValue * deltaTime;
        float deltaY = _velocity * deltaTime;
        return new Vector3(0f, deltaY, 0f);
    }
}