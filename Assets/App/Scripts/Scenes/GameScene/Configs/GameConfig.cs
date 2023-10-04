using UnityEngine;

[CreateAssetMenu(fileName = "new GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Gravitation Settings")]
    public float UpGravitationalConstant = -9f;
    public float DownGravitationalConstant = -6f;
    [Tooltip("Speed at which gravity changes")]
    public float BoundaryVelocity = 0.5f;
    [Header("DestroyLine Settings")]
    public float DestroyLineYOffset = 2f;
    [Header("Shooting Settings")]
    public float ShootForce = 1f;
}
