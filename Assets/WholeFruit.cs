using UnityEngine;

public class WholeFruit : MonoBehaviour, ISlicable
{
    [field: SerializeField] public FruitPart LeftFruitPart;
    [field: SerializeField] public FruitPart RightFruitPart;

    private Shooter _shooter;
    public void Construct(Shooter shooter)
    {
        _shooter = shooter;
    }

    public void Slice(Vector2 sliceVector)
    {
        _shooter.ShootFruit(LeftFruitPart.gameObject, new Vector2(sliceVector.y, -sliceVector.x) * 3f);
        _shooter.ShootFruit(RightFruitPart.gameObject, new Vector2(-sliceVector.y, sliceVector.x) * 3f);
    }
}
