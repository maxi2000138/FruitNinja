using System;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

public class DestroySliceObject : MonoBehaviour, IFullSliceObject
{
    public event Action OnSliceEvent;

    [field: SerializeField] 
    public ProjectileType ProjectileType { get; private set; }
    [field: SerializeField]
    public ISlicable Slicable { get; private set; }
    [field: SerializeField] 
    public ProjectileObject ProjectileObject { get; private set; }

    private IDestroyTrigger _destroyTrigger;

    private void Awake()
    {
        Slicable = GetComponent<ISlicable>();
    }

    public void Construct(IDestroyTrigger destroyTrigger)
    {
        _destroyTrigger = destroyTrigger;
    }


    public void Slice(Mover mover, float sliceForces, out bool disableColliderOnSlice)
    {
        OnSliceEvent?.Invoke();
        Slicable.OnSlice();
        disableColliderOnSlice = true;
        _destroyTrigger.TriggerGroup(ProjectileObject);
    }
}
