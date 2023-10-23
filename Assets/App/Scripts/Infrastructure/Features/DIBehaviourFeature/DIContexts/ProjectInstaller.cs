using System.Collections.Generic;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.CoroutineRunner;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using CodeBase.Infrastructure;
using CodeBase.Logic;
using UnityEngine;

public class ProjectInstaller : InstallerBehaviour
{
    [field:SerializeField] 
    public CoroutineRunner CoroutineRunner { get; private set; }
    [field:SerializeField] 
    public LoadingCurtain LoadingCurtain { get; private set; }
    [SerializeField] 
    private SaveDataKeysConfig _saveDataKeysConfig;
    public SaveLoadService SaveLoadService { get; private set; }
    public PersistantDataSaver PersistantDataSaver { get; private set; }
    public List<ISaveDataContainer> SavedDataContainers { get; private set; }
    public List<ISavedTrigger> SavedTriggers { get; private set; }
    public SaveDataContainer<ScoreData> ScoreStateContainer { get; private set; }
    public SaveTimeTrigger SaveTimeTrigger { get; private set; }
    public SceneLoaderWithCurtains SceneLoaderWithCurtains { get; private set; }
    public TweenCore TweenCore { get; private set; }
    
    private SceneLoader SceneLoader;
    

    public override void OnInstallBindings(MonoBehaviourSimulator monoBehaviourSimulator, ProjectInstaller projectInstaller)
    {
        TweenCore = new TweenCore();
        
        SaveLoadService = new SaveLoadService();
        
        ScoreStateContainer = new SaveDataContainer<ScoreData>(SaveLoadService, _saveDataKeysConfig.GetDataKey<ScoreData>());
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
        
        SceneLoader = new SceneLoader(CoroutineRunner);
        LoadingCurtain.Construct(TweenCore);
        SceneLoaderWithCurtains = new SceneLoaderWithCurtains(SceneLoader, LoadingCurtain);
        
        monoBehaviourSimulator.AddInitializable(PersistantDataSaver);
        monoBehaviourSimulator.AddUpdatable(SaveTimeTrigger);
        
    }
}
