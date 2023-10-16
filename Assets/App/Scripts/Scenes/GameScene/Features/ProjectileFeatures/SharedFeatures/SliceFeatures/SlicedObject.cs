using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Rotater;
using UnityEngine;

public class SlicedObject : MonoBehaviour, ISliced
{
    [field: SerializeField] public Transform Transform { get; private set; }
    [field: SerializeField] public VelocityApplier VelocityApplier { get; private set; }
    public TorqueApplier TorqueApplier { get; }
    [field: SerializeField] public ResultForceRotater ForceRotater { get; private set; }
}
