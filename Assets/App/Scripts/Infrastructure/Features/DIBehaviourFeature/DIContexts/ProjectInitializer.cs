using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

public class ProjectInitializer : MonoBehaviour
{
    private static MonoBehaviourSimulator _monoBehaviourSimulator;
    
    private const string PROJECT_COMPOSITE_ROOT_PATH = "[PROJECT_COMPOSITE_ROOT]";

    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad()
    {
        InstallerBehaviour installerPrefab = ((GameObject)Resources.Load(PROJECT_COMPOSITE_ROOT_PATH)).GetComponent<InstallerBehaviour>();
        InstallerBehaviour installer = Instantiate(installerPrefab);
        _monoBehaviourSimulator = installer.GetComponentInChildren<MonoBehaviourSimulator>();
        DontDestroyOnLoad(installer.transform.root);
        installer.InstallBindings(_monoBehaviourSimulator);
        _monoBehaviourSimulator.InitializeAll();
        SetGameSettings();
    }
    
    private static void SetGameSettings()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
