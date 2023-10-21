using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new ComboConfig", menuName = "Configs/Combo Config")]
public class ComboConfig : ScriptableObject
{
    [FormerlySerializedAs("_comboPrefab")] public ComboPrefab ComboPrefab;
    [FormerlySerializedAs("_scale")] public Vector3 Scale;
    [Range(0.1f, 1f)]
    public float DelayComboDestroy = 0.5f;
    [Range(0.5f, 3f)] 
    public float ComboLifeTime = 1f;
}
