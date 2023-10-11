using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Vector2 SpriteOffset =>
        SpriteRenderer.transform.localPosition;
    [field: SerializeField] public GameObject SpriteGameObject { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public ScaleByTime ScaleByTime { get; private set; }
    [field: SerializeField] public OffsetByTime OffsetByTime { get; private set; }
    
    private ShadowConfig _shadowConfig;

    public void Construct(ShadowConfig shadowConfig)
    {
        _shadowConfig = shadowConfig;
    }
    
    public void SetSpriteWithOffset(Sprite sprite, Vector2 spriteScale, float distance)
    {
        SpriteRenderer.transform.localScale = spriteScale;
        SpriteRenderer.sprite = sprite;
        SetSpriteOffset(spriteScale.magnitude * distance);
    }

    public void SetSpriteOffset(float distance)
    {
        Vector2 offsetVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY);
        offsetVector = offsetVector.normalized * distance;
        SpriteRenderer.transform.localPosition =
            new Vector3(offsetVector.x, offsetVector.y, 0f);
    }

    public void TurnIntoShadow()
    {
        Color32 shadowColor = Color.black;
        shadowColor.a = 100;
        SpriteRenderer.color = shadowColor;
    }
}
