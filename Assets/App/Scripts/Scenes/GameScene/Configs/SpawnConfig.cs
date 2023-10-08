using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnConfig", menuName = "Configs/Spawn Config")]
public class SpawnConfig : ScriptableObject
{
    [MinMaxSlider(1,10)]
    public Vector2Int FruitsAmountRange;
    [MinMaxSlider(1f,10f)]
    public Vector2 GroupSpawnDelayRange;
    [MinMaxSlider(0f,2f)]
    public Vector2 FruitsInGroupSpawnDelayRange;

}
