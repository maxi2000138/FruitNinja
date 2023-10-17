using System.Collections.Generic;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

public class ProjectInstaller : InstallerBehaviour
{
    [SerializeField] 
    private SaveDataKeysConfig _saveDataKeysConfig;
    
    public SaveLoadService SaveLoadService { get; private set; }
    public PersistantDataSaver PersistantDataSaver { get; private set; }
    public List<ISaveDataContainer> SavedDataContainers { get; private set; }
    public List<ISavedTrigger> SavedTriggers { get; private set; }
    public SaveDataContainer<ScoreState> ScoreStateContainer { get; private set; }
    public SaveTimeTrigger SaveTimeTrigger { get; private set; }
    
    public override void InstallBindings(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        SaveLoadService = new SaveLoadService();
        
        ScoreStateContainer = new SaveDataContainer<ScoreState>(SaveLoadService, _saveDataKeysConfig.GetDataKey<ScoreState>());
        SaveTimeTrigger = new SaveTimeTrigger();
        
        SavedTriggers = new List<ISavedTrigger>()
        {
            SaveTimeTrigger,   
        };
        SavedDataContainers = new List<ISaveDataContainer>()
        {
            ScoreStateContainer,
        };
        
        PersistantDataSaver = new PersistantDataSaver(SavedDataContainers,SavedTriggers);
        
        monoBehaviourSimulator.AddInitializable(PersistantDataSaver);
        monoBehaviourSimulator.AddUpdatable(SaveTimeTrigger);
    }
}
