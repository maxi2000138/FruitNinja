using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

public class DestroySliceObject : MonoBehaviour, IFullSliceObject
{
    [field: SerializeField] 
    public ProjectileType ProjectileType { get; private set; }
    [field: SerializeField] 
    public ProjectileObject ProjectileObject { get; private set; }

    private ISlicable _slicable;
    private IDestroyTrigger _destroyTrigger;

    public void Construct(ISlicable slicable, IDestroyTrigger destroyTrigger)
    {
        _destroyTrigger = destroyTrigger;
        _slicable = slicable;
    }

    
    public void Slice(Mover mover, float sliceForce)
    {
        _slicable.OnSlice();
        _destroyTrigger.TriggerGroup(ProjectileObject);
    }
}
