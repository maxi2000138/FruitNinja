using UnityEngine;

[CreateAssetMenu(fileName = "new GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public float GravitationalConstant = 9.81f;
    public float DestroyLineYOffset = 2f;
    public float ShootForce = 1f;
}
