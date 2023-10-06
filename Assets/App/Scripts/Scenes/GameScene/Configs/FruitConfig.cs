using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new FruitConfig", menuName = "Configs/Fruit Config")]
public class FruitConfig : SerializedScriptableObject
{
    public Dictionary<FruitType, FruitData> FruitDictionary;
}

[Serializable]
public class FruitData
{
    [Header("Sprite Settings")]
    public Sprite Sprite;
    public Vector2 SpriteScale = new(1,1);
}