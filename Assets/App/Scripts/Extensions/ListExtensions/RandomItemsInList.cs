using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.ListExtensions
{
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
        
        public static float GetRandomBound(this Vector2 vector2)
        {
            var randomIndex = Random.value;
            if(randomIndex >= 0.5f)
                return vector2.y;
            return vector2.x;
        }
    
        public static T GetRandomItem<T>(this List<T> list)
        {
            T randomItem = (T)list[(int)(Random.value * (list.Count-1))];
            return randomItem;
        }

        public static Vector2 GetRandomPointBetween(this (Vector2, Vector2) items)
        {
            float randomIndex = Random.value;
            Vector2 randomPoint = (items.Item2 - items.Item1) * randomIndex + items.Item1;
            return randomPoint;
        }
    }
}