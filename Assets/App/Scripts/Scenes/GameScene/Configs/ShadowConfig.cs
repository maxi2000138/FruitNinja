using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new ShadowConfig", menuName = "Configs/Shadow Config")]
public class ShadowConfig : ScriptableObject
{
    [Range(-1f,1f)]
    public float ShadowDirectionX;
    [Range(-1f,1f)]
    public float ShadowDirectionY;
    [MinMaxSlider(0f,2f)]
    public Vector2 ShadowScaleRange;
}
