using UnityEngine;

public class ChangeData
{
    public Vector2 StartValue;
    public Vector2 CurrentValue;
    public Vector2 FinalValue;
    public float FlyTime;
    public float CurrentTime;

    public ChangeData(Vector2 startValue, Vector2 finalValue, float flyTime)
    {
        StartValue = startValue;
        FinalValue = finalValue;
        FlyTime = flyTime;
    }
}