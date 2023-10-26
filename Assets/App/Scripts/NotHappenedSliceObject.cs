using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using Sirenix.Serialization;
using UnityEngine;

public class NotHappenedSliceObject : MonoBehaviour, IFullSliceObject
{
    [field: SerializeField] 
    public ProjectileType ProjectileType { get; private set; }
    [field: SerializeField] 
    public ISlicable Slicable { get; private set; }

    [field: SerializeField] 
    public ProjectileObject ProjectileObject { get; private set; }


    private void Awake()
    {
        Slicable = GetComponent<ISlicable>();
    }

    public void Slice(Mover mover, float sliceForces, out bool disableColliderOnSlice)
    {
        Slicable.OnSlice();
        disableColliderOnSlice = false;
    }

}
