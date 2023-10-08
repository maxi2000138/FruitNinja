using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomItemInList
{
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
        var randomIndex = Random.value * (items.Item2 - items.Item1) + items.Item1;
        return randomIndex;
    }

    public static float GetRandomFloatBetween(this Vector2 vector2)
    {
        var randomIndex = Random.value * (vector2.y - vector2.x) + vector2.x;
        return randomIndex;
    }
    
    public static int GetRandomIntBetween(this Vector2Int vector2)
    {
        int randomIndex = (int)(Random.value * (vector2.y - vector2.x) + vector2.x);
        return randomIndex;
    }

    public static int GetRandomIntBetween(this (int, int) items)
    {
        int randomIndex = (int)(Random.value * (items.Item2 - items.Item1) + items.Item1);
        return randomIndex;
    }

    public static Vector2 GetRandomPointBetween(this (Vector2, Vector2) items)
    {
        float randomIndex = Random.value;
        Vector2 randomPoint = (items.Item2 - items.Item1) * randomIndex + items.Item1;
        return randomPoint;
    }
}