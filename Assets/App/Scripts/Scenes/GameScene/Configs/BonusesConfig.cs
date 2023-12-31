using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "new BonusesConfig", menuName = "Configs/Bonuses Config")]
public class BonusesConfig : SerializedScriptableObject
{
    [Header("Magnet")] 
    public float MagnetTime = 5f;
    public float SuctionEndTime = 0.5f;
    public float FarthestMagnetDistance = 30f;
    public float ForceVelocity = 1f;
    public float ScaleFactor = 10000f;
    public float StopingDistance = 1f;
    public float DirectionScaleFactor = 0.1f;
    public float scaleLitleFactor = 1f;
    public float ScaleFactorDistance = 1f;
    [Header("Frozen")] 
    public float FrozenTimeScale = 0.5f;
    public float FrozenTime = 8f;
    [Header("Mimik")] 
    [FormerlySerializedAs("ChangeTime")] public float MimikChangeTime = 1f;
    public float MimikBeforeChangeParticleDeltaTime = 0.5f;
    [Header("StringBag")] 
    public float StringBagFruitsAmount = 5f;
    public float StringBagFruitsAngleRange = 15f;
    public float StringBagInivisibleTime = 1f;
    public float StringBagFruitForce = 25f;
    [Header("Samurai")] 
    public float SamuraiTime = 10f;
    public float FruitsIncreaseValue = 3f;
    public float PackDeltaTimeDecreaseValue = 3f;
}

