using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    [field: SerializeField] public ProjectileObject ProjectileObject;
    [field: SerializeField] public ScaleByTimeApplier ScaleByTimeApplier;
    [field: SerializeField] public TorqueApplier TorqueApplier;
    [field: SerializeField] public VelocityApplier VelocityApplier;
}
