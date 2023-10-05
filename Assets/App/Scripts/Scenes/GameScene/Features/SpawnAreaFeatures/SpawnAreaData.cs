using System;
using UnityEngine;

[Serializable]
public class SpawnAreaData
{
    [Header("Position")]
    [Range(0,1)]
    public float ViewportPositionX;
    [Range(0,1)]
    public float ViewportPositionY;
    [HideInInspector] 
    public Vector2 ViewportLeftPosition;
    [HideInInspector] 
    public Vector2 ViewportRightPosition;
    [Header("Length")]
    [Range(0,1)] 
    public float LineLength;
    [Header("Angles")]
    [Range(0,360)] 
    public float LineAngle;
    [Range(0,180)] 
    public float ShootMinAngle;
    [Range(0,180)] 
    public float ShootMaxAngle;
    [Header("Probability")]
    [Range(0, 1)] 
    public float Probability;
}
