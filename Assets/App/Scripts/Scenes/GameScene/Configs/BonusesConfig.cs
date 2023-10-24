using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new BonusesConfig", menuName = "Configs/Bonuses Config")]
public class BonusesConfig : SerializedScriptableObject
{
    public float MagnetTime = 5f;
}

