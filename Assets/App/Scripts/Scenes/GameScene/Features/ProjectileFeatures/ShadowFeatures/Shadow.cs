using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures
{
    public class Shadow : MonoBehaviour
    {
        public Vector2 Offset => SpriteRenderer.transform.localPosition;
        public Vector2 Scale => transform.localScale;
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public ScaleByTimeApplier ScaleByTimeApplier { get; private set; }
        [field: SerializeField] public OffsetByTimeApplier OffsetByTimeApplier { get; private set; }
    
    
        public void SetSpriteWithOffsetAndScale(Sprite sprite, Vector2 offsetVector, Vector2 scale)
        {
            transform.localScale = scale;
            SpriteRenderer.sprite = sprite;
            SetSpriteOffset(offsetVector);
        }

        public void SetSpriteOffset(Vector2 shadowVector)
        {
            SpriteRenderer.transform.localPosition =
                new Vector3(shadowVector.x, shadowVector.y, 0f);
        }

        public void TurnIntoShadow()
        {
            Color32 shadowColor = Color.black;
            shadowColor.a = 100;
            SpriteRenderer.color = shadowColor;
        }
    }
}
