using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public interface IFullSliceObject
    {
        public ProjectileType ProjectileType { get;  }
        public ISlicable Slicable { get;  }
        void Slice(Mover mover, float sliceForce);
    }
}