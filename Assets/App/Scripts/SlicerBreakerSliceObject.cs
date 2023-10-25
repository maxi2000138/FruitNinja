using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using UnityEngine;

public class SlicerBreakerSliceObject : MonoBehaviour, IFullSliceObject
{
    [field: SerializeField] 
    public ProjectileType ProjectileType { get; private set; }
    [field: SerializeField] 
    public ProjectileObject ProjectileObject { get; private set; }


    private Slicer _slicer;

    public void Construct(Slicer slicer)
    {
        _slicer = slicer;
    }
    
    public void Slice(Mover mover, float sliceForce)
    {
        _slicer.EndSlicing();
    }
}
