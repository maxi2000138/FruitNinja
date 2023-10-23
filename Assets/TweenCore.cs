using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TweenCore
{
    private readonly int _deltaTime = 10;

    private readonly Dictionary<CustomEase, Func<float, float>> _easeFunctions = new()
    {
        [CustomEase.Linear] = easeLinear,
        [CustomEase.OutQuad] = easeOutQuad,
        [CustomEase.FullCosine] = easeFullCosine,
    };

    public TweenCore()
    {
    }
    
    public async Task TweenByTime(Action<float> setAction, float startValue, float endValue, float time, CustomEase ease, CancellationToken token)
    {
        Func<float, float> easeFunction = _easeFunctions[ease];
        float currentTime = 0f;
        float startTime;
        
        setAction?.Invoke(startValue);

        while (currentTime < time)
        {
            setAction?.Invoke(ToCustomRange(startValue, endValue,easeFunction(Mathf.Lerp(0f, 1f, currentTime/time))));
            startTime = Time.time;
            await Task.Delay(_deltaTime, token);
            currentTime += (Time.time - startTime);
        }
        
        setAction?.Invoke(endValue);
    }
    
    public async Task PunchByTime(Action<Vector3> setAction, Vector3 startValue, Vector3 endValue, float time, CustomEase ease, CancellationToken token)
    {
        Func<float, float> easeFunction = _easeFunctions[ease];
        float currentTime = 0f;
        float startTime;
        
        setAction?.Invoke(startValue);

        while (currentTime < time)
        {
            setAction?.Invoke(ToCustomRange(startValue, endValue,easeFunction(Mathf.Lerp(0f, 1f, currentTime / time))));
            startTime = Time.time;
            await Task.Delay(_deltaTime, token);
            currentTime += (Time.time - startTime);
        }
        
        setAction?.Invoke(startValue);
    }
    

    private float ToCustomRange(float startValue, float endValue, float normalizedValue)
    {
        return (endValue - startValue) * normalizedValue + startValue;
    }
    
    private Vector3 ToCustomRange(Vector3 startValue, Vector3 endValue, float normalizedValue)
    {
        
        return new Vector3(ToCustomRange(startValue.x, endValue.x, normalizedValue), ToCustomRange(startValue.y, endValue.y, normalizedValue), ToCustomRange(startValue.z, endValue.z, normalizedValue));
    }
    
    private static float easeOutQuad(float x) 
    {
        return 1 - (1 - x) * (1 - x);
    }
    
    private static float easeFullCosine(float x) 
    {
        return Mathf.Cos(x*Mathf.PI-Mathf.PI/2);
    }
    
    private static float easeLinear(float x) 
    {
        return x;
    }
}

public enum CustomEase
{
    Linear,
    OutQuad,
    FullCosine,
}

