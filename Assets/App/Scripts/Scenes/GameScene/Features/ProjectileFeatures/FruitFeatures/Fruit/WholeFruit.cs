using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.PhysicsFramework;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit
{
    public class WholeFruit : MonoBehaviour
    {
        [field: SerializeField] public FruitPart LeftFruitPart;
        [field: SerializeField] public FruitPart RightFruitPart;
        [field: SerializeField] public ScaleByTimeApplier ScalerByTimeApplier;
        [field: SerializeField] public TorqueApplier TorqueApplier;
        [field: SerializeField] public PhysicsForcesOrder PhysicsForcesOrder;
        [field: SerializeField] public ParticleSystemController SliceParticlesController;
    }
}
