using UnityEngine;

public class FruitPart : MonoBehaviour
{
    public float SpriteMaxHeight => SpriteDiagonal() * _spriteScale.y;
    public Vector2 SpriteScale => _spriteRenderer.transform.localScale;
    public Shadow Shadow { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CloneMover _shadowMover;
    [SerializeField] private CloneRotater _shadowRotater;
    [SerializeField] private Vector2 _spriteScale;
    
    private void OnDestroy()
    {
        /*
        _transformLocalScaler.StopChanging(transform);
        _transformLocalScaler.StopChanging(Shadow.SpriteRenderer.transform);
        _transformLocalOffseter.StopChanging(Shadow.SpriteRenderer.transform);
    */
    }

    public void SetSprite(Sprite sprite, Vector2 spriteScale, int sortingOrder)
    {
        _spriteScale = spriteScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.sortingOrder = sortingOrder;
        ChangeSpriteScale(spriteScale);
    }

    public void SetShadow(Shadow shadow)
    {
        Shadow = shadow;
        _shadowMover.Construct(shadow.gameObject);
        _shadowRotater.Construct(shadow.SpriteGameObject);
    }

    public void StartChangingShadowSpriteScale(Vector2 startScale, Vector2 finalScale, float flyTime)
    {
       Shadow.ScaleByTime.StartScaling(startScale, finalScale, flyTime);
    }

    public void StartChangingShadowOffset(Vector2 startOffset, Vector2 finalOffset, float flyTime)
    {
        Shadow.OffsetByTime.StartOffseting(startOffset,finalOffset, flyTime);
    }

    private void ChangeSpriteScale(Vector2 spriteScale)
    {
        _spriteRenderer.transform.localScale = spriteScale;
    }

    private float SpriteDiagonal() => Mathf.Sqrt(Mathf.Pow(_spriteRenderer.sprite.bounds.size.x,2) + Mathf.Pow(_spriteRenderer.sprite.bounds.size.y,2));
}