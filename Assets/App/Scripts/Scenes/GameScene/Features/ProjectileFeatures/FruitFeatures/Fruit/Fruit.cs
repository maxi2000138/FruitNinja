using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float SpriteMaxHeight => SpriteDiagonal() * _spriteScale.y;
    public float SpriteScale => _spriteRenderer.transform.localScale.x;
    [field: SerializeField] public Shadow Shadow { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CloneMover _shadowMover;
    [SerializeField] private CloneRotater _shadowRotater;
    private SpriteOffseter _shadowSpriteOffseter;
    private ICoroutineRunner _coroutineRunner;
    private ShadowConfig _shadowConfig;
    private SpriteScaler _fruitSpriteScaler;
    private SpriteScaler _shadowSpriteScaler;
    private Vector2 _spriteScale;

    public void Construct(ICoroutineRunner coroutineRunner, ShadowConfig shadowConfig)
    {
        _shadowConfig = shadowConfig;
        _coroutineRunner = coroutineRunner;
        _fruitSpriteScaler = new SpriteScaler(_spriteRenderer, coroutineRunner);
    }

    private void OnDestroy()
    {
        _shadowSpriteScaler.StopScaling();
        _fruitSpriteScaler.StopScaling();
        _shadowSpriteOffseter.StopOffseter();
    }

    public void SetSprite(Sprite sprite, Vector2 spriteScale)
    {
        _spriteScale = spriteScale;
        _spriteRenderer.sprite = sprite;
        ChangeSpriteScale(spriteScale);
    }

    public void SetShadow(Shadow shadow)
    {
        Shadow = shadow;
        _shadowMover.Construct(shadow.gameObject);
        _shadowRotater.Construct(shadow.SpriteGameObject);
        _shadowSpriteScaler = new SpriteScaler(Shadow.SpriteRenderer, _coroutineRunner);
        _shadowSpriteOffseter = new SpriteOffseter(Shadow.SpriteRenderer, _coroutineRunner);
    }

    public void StartChangingFruitSpriteScale(float deltaScale, float flyTime)
    {
        _fruitSpriteScaler.StartScaling(deltaScale, flyTime);
    }
    
    public void StartChangingShadowSpriteScale(float deltaScale, float flyTime)
    {
        _shadowSpriteScaler.StartScaling(deltaScale, flyTime);
    }


    public void StartChangingShadowOffset(Vector2 shadowOffset, float finalOffset, float flyTime)
    {
        _shadowSpriteOffseter.StartOffseter(shadowOffset, finalOffset, flyTime);   
    }

    private void ChangeSpriteScale(Vector2 spriteScale)
    {
        _spriteRenderer.transform.localScale = spriteScale;
    }

    private float SpriteDiagonal() => Mathf.Sqrt(Mathf.Pow(_spriteRenderer.sprite.bounds.size.x,2) + Mathf.Pow(_spriteRenderer.sprite.bounds.size.y,2));
}