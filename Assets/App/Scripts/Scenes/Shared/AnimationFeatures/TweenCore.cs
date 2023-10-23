using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TweenCore
{
    private readonly int _deltaTime = 10;
    
    private static readonly FloatInterpolatable _floatInterpolatable = new ();
    private static readonly Vector3Interpolatable _vector3Interpolatable = new (_floatInterpolatable);

    private readonly Dictionary<CustomEase, Func<float, float>> _easeFunctions = new()
    {
        [CustomEase.Linear] = easeLinear,
        [CustomEase.OutQuad] = easeOutQuad,
        [CustomEase.FullCosine] = easeFullCosine,
    };
    
    private Dictionary<Type, object> _interpolatableStrategies = new()
    {
        { typeof(float), _floatInterpolatable },
        { typeof(Vector3), _vector3Interpolatable }
    };
    
    public async UniTask TweenByTime<T>(Action<T> setAction, T startValue, T endValue, float time, CustomEase ease, CancellationToken token)
    {
        setAction?.Invoke(startValue);
        
        await TweenTimeLogic(setAction, startValue, endValue, time, ease, token);
        
        setAction?.Invoke(endValue);
    }
    
    public async UniTask PunchByTime<T>(Action<T> setAction, T startValue, T endValue, float time, CustomEase ease, CancellationToken token)
    {
        setAction?.Invoke(startValue);
        
        await TweenTimeLogic(setAction, startValue, endValue, time, ease, token);

        setAction?.Invoke(startValue);
    }
    

    private async UniTask TweenTimeLogic<T>(Action<T> setAction, T startValue, T endValue, float time, CustomEase ease, CancellationToken token)
    {
        Func<float, float> easeFunction = _easeFunctions[ease];
        float currentTime = 0f;
        float startTime;
        IInterpolatable<T> interpolatable = GetInterpolatable<T>();

        while (currentTime < time)
        {
            float normalizedValue = easeFunction(Mathf.Lerp(0f, 1f, currentTime / time));

            setAction?.Invoke(interpolatable.Interpolate(startValue, endValue, normalizedValue));

            startTime = Time.realtimeSinceStartup;
            await UniTask.Delay(_deltaTime, true, PlayerLoopTiming.Update, token);
            currentTime += Time.realtimeSinceStartup - startTime;
        }
    }
    
    private IInterpolatable<T> GetInterpolatable<T>()
    {
        if (_interpolatableStrategies.TryGetValue(typeof(T), out var strategy))
        {
            return (IInterpolatable<T>)strategy;
        }

        return null;
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

public interface IInterpolatable<T>
{
    T Interpolate(T start, T end, float normalizedValue);
}

public class FloatInterpolatable : IInterpolatable<float>
{
    public float Interpolate(float start, float end, float normalizedValue)
    {
        return (end - start) * normalizedValue + start;
    }
}

public class Vector3Interpolatable : IInterpolatable<Vector3>
{
    private readonly FloatInterpolatable _floatInterpolatable;

    public Vector3Interpolatable(FloatInterpolatable floatInterpolatable)
    {
        _floatInterpolatable = floatInterpolatable;
    }
    
    public Vector3 Interpolate(Vector3 start, Vector3 end, float normalizedValue)
    {
        return new Vector3(
            _floatInterpolatable.Interpolate(start.x, end.x, normalizedValue),
            _floatInterpolatable.Interpolate(start.y, end.y, normalizedValue),
            _floatInterpolatable.Interpolate(start.z, end.z, normalizedValue)
        );
    }
}


