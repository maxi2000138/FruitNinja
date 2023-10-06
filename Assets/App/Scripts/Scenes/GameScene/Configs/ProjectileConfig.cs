using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new ProjectileConfig", menuName = "Configs/Projectile Config")]
public class ProjectileConfig : ScriptableObject
{
    [MinMaxSlider(0f,50f)]
    public Vector2 ShootVelocityRange = new (1f,1f);
    [MinMaxSlider(-1000f,1000f)]
    public Vector2 TorqueVelocityRange = new (1f,1f);
    [Range(-10f,0f)]
    public float DestroyTriggerOffset = 2f;
}
