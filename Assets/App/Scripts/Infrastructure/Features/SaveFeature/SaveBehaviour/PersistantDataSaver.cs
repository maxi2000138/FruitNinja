using System.Collections.Generic;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

public class PersistantDataSaver : IInitializable
{
    private readonly IEnumerable<ISaveDataContainer> _saveDataContainers;
    private readonly IEnumerable<ISavedTrigger> _savedTriggers;

    public PersistantDataSaver(IEnumerable<ISaveDataContainer> saveDataContainers, IEnumerable<ISavedTrigger> savedTriggers)
    {
        _saveDataContainers = saveDataContainers;
        _savedTriggers = savedTriggers;
    }

    public void Initialize()
    {
        LoadAll();

        foreach (ISavedTrigger trigger in _savedTriggers)
        {
            trigger.NeedSave += SaveAll;
        }
    }

    private void SaveAll()
    {
        foreach (ISaveDataContainer saveDataContainer in _saveDataContainers)
        {
            if(saveDataContainer.IsChanged)
                saveDataContainer.Save();
        }
    }

    private void LoadAll()
    {
        foreach (ISaveDataContainer saveDataContainer in _saveDataContainers)
        {
            saveDataContainer.Load();
        }
    }
}
