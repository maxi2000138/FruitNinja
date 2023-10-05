using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new FruitConfig", menuName = "Configs/Fruit Config")]
public class FruitConfig : ScriptableObject
{
    public List<FruitData> FruitDatas;
}

[Serializable]
public class FruitData
{
    public FruitType FruitType;
    public Sprite Sprite;
}
