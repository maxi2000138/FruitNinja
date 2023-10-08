using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new ShadowConfig", menuName = "Configs/Shadow Config")]
public class ShadowConfig : ScriptableObject
{
    [Range(-1f,1f)]
    public float ShadowDirectionX;
    [Range(-1f,1f)]
    public float ShadowDirectionY;
    [Range(0f,10f)]
    public float ShadowOffsetScaler;
    [Range(0f,10f)]
    public float ShadowScaleScaler;

}
