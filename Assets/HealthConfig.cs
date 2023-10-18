using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "new HealthConfig", menuName = "Configs/Health Config")]
public class HealthConfig : ScriptableObject
{
    public int Health = 3;
    public float SpriteOffset;
    public float UpOffset;
    public float RightOffset;
    public GameObject HeartPrefab;
}
