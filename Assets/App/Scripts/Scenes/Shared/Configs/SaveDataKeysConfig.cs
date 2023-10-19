using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "new SaveDataKeysConfig", menuName = "Configs/SaveData Keys Config")]
public class SaveDataKeysConfig : SerializedScriptableObject
{
    [OdinSerialize] private Dictionary<ISavedData, string> SaveDataKeys;

    public string GetDataKey<T>() where T : ISavedData
    {
        foreach (ISavedData saveDataKey in SaveDataKeys.Keys)
        {
            if (saveDataKey.GetType() == typeof(T))
                return SaveDataKeys[saveDataKey];
        }

        return null;
    }
}
