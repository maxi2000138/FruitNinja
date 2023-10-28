using System;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures
{
    public interface IFullSliceObject
    {
        public event Action OnSliceEvent; 
        public ProjectileType ProjectileType { get;  }
        public ProjectileObject ProjectileObject { get;  }
        public ISlicable Slicable { get;  }
        void Slice(Mover mover, float sliceForces, out bool disableColliderOnSlice);
    }
}