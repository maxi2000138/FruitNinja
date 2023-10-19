using UnityEngine;

public static class JsonExtensions
{
    public static string ToJson<T>(this T data)
    {
        return JsonUtility.ToJson(data);
    }
    
    public static T FromJson<T>(this string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }
}
