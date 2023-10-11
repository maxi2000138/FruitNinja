using UnityEngine;

public class WholeFruit : MonoBehaviour
{
    [field: SerializeField] public FruitPart LeftFruitPart;
    [field: SerializeField] public FruitPart RightFruitPart;
    [field: SerializeField] public ScaleByTime ScalerByTime;
    [field: SerializeField] public TorqueApplier TorqueApplier;
    [field: SerializeField] public PhysicsOperationOrder PhysicsOperationOrder;
}
