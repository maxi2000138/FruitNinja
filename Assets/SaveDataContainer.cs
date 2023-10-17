using System;
using UnityEngine;

public class SaveDataContainer<T> : ISaveDataContainer<T> where T : ISavedData, new()
{
    public event Action DataLoaded;
    public bool IsChanged { get; private set; }
    
    private readonly ISaveLoadService _saveLoadService;
    private readonly string KEY;
    private T DataValue;



    public SaveDataContainer(ISaveLoadService saveLoadService, string key)
    {
        _saveLoadService = saveLoadService;
        KEY = key;
    }

    public T WriteData()
    {
        IsChanged = true;
        return DataValue;
    }

    
    public T ReadData()
    {
        return DataValue;
    }
    
    public void Load()
    {
        Debug.Log("Load: " + KEY);
        T data = _saveLoadService.LoadProgress<T>(KEY);
        if (data != null)
        {
            DataValue = data;
            IsChanged = true;
            DataLoaded?.Invoke();
        }
        else
            DataValue = new T();
    }

    public void Save()
    {
        Debug.Log("Save: " + KEY);
        _saveLoadService.SaveProgress(DataValue, KEY);
        IsChanged = false;
    }
}
