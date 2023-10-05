using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new ProjectileConfig", menuName = "Configs/Projectile Config")]
public class ProjectileConfig : ScriptableObject
{
    [MinMaxSlider(0f,50f)]
    public Vector2 ShootVelocity = new (1f,1f);
    [MinMaxSlider(-1000f,1000f)]
    public Vector2 TorqueVelocity = new (1f,1f);
    [Range(0,10f)]
    public float ShadowDistance = 1f;
}
