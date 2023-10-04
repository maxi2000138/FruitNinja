using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomItemInList
{
    private const int MinRandomIndex = 0;
    
    public static T GetRandomItemByProbability<T>(this IEnumerable<T> list, Func<T, float> item)
    {
        var sum = list.Sum(item);
        
        var randomPoint = Random.value * sum;

        foreach (var arg in list)
        {
            var prob = item(arg);
            if (randomPoint < prob)
            {
                return arg;
            }
            else
            {
                randomPoint -= prob;
            }
        }

        return list.Last();
    }

    public static float GetRandomFloatBetween(this (float, float) items)
    {
        var randomIndex = Random.Range(items.Item1, items.Item2);
        return randomIndex;
    }
    
    public static Vector2 GetRandomPointBetween(this (Vector2, Vector2) items)
    {
        var randomIndex = Random.Range(0f,1f);
        Vector2 randomPoint = items.Item1 + (items.Item2 - items.Item1) * randomIndex;
        return randomPoint;
    }
}