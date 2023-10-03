using System;
using UnityEngine;

[Serializable]
public class SpawnAreaData
{
    [Range(0,1)] public float ViewportPositionX;
    [Range(0,1)] public float ViewportPositionY;
    [Range(0,1)] public float LineLength;
    [Range(0,360)] public float LineAngle;
    [Range(0,180)] public float ShootMinAngle;
    [Range(0,180)] public float ShootMaxAngle;
}
