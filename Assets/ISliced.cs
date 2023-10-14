using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using UnityEngine;

public interface ISliced
{
    Transform Transform { get; }
    ForceApplier ForceApplier { get;  }
    ResultForceRotater ForceRotater { get;  }
}
