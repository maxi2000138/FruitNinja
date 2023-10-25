using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

public class NothingHappendSliceObject : IFullSliceObject
{
    [field: SerializeField] 
    public ProjectileType ProjectileType { get; private set; }
    [field: SerializeField] 
    public ProjectileObject ProjectileObject { get; private set; }

    private ISlicable _slicable;

    public void Construct(ISlicable slicable)
    {
        _slicable = slicable;
    }
    
    public void Slice(Mover mover, float sliceForce)
    {
        _slicable.OnSlice();       
    }
}
