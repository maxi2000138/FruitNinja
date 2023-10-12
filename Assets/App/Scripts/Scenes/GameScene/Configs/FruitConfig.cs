using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new FruitConfig", menuName = "Configs/Fruit Config")]
public class FruitConfig : SerializedScriptableObject
{
    public Dictionary<FruitType, FruitData> FruitDictionary;
    [MinMaxSlider(0f, 2f)]
    public Vector2 FruitScaleRange;
}

[Serializable]
public class FruitData
{
    public Sprite LeftSprite;
    public Sprite RightSprite;
    public Color SliceColor;
}
