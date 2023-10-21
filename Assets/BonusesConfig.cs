using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new BonusesConfig", menuName = "Configs/Bonuses Config")]
public class BonusesConfig : SerializedScriptableObject
{
    public Dictionary<BonusType, BonusData> BonusData;
}

[Serializable]
public class BonusData
{
    public Dictionary<ProjectilePartEnum, Sprite> PartSprites;
    [MinMaxSlider(0f, 2f)]
    public Vector2 Scale;
}

public enum BonusType
{
    Bomb,
    Heart,
}
