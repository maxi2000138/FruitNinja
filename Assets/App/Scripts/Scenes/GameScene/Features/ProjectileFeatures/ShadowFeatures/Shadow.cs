using UnityEngine;

public class Shadow : MonoBehaviour
{
    [field: SerializeField] public GameObject SpriteGameObject { get; private set; }
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private ShadowConfig _shadowConfig;

    public void Construct(ShadowConfig shadowConfig)
    {
        _shadowConfig = shadowConfig;
    }
    
    public void SetSpriteWithOffset(Sprite sprite, Vector2 spriteScale)
    {
        _spriteRenderer.transform.localScale = spriteScale;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.transform.localPosition = new Vector3(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY);
    }
    
    public void TurnIntoShadow()
    {
        Color32 shadowColor = Color.black;
        shadowColor.a = 100;
        _spriteRenderer.color = shadowColor;
    }
}
