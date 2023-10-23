using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    public void SaveProgress<T>(T state, string key)
    {
        PlayerPrefs.SetString(key, state.ToJson());
    }

    public T LoadProgress<T>(string key)
    {
        return PlayerPrefs.GetString(key).FromJson<T>();
    }
}
