using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

public class MenuServicesInstaller : InstallerBehaviour
{

    [SerializeField] private MenuHighScoreController _menuHighScoreController;
    [SerializeField] private MenuEntryPoint _menuEntryPoint;
    [SerializeField] private CustomButton _playButtonBehaviour;
    [SerializeField] private CustomButton _exitButtonBehaviour;
    [SerializeField] private ScoreView _highScoreView;

    public override void OnInstallBindings(MonoBehaviourSimulator monoBehaviourSimulator, ProjectInstaller projectInstaller)
    {
        if (Application.isEditor)
            _exitButtonBehaviour.Construct(new EditorExitButton());
        else
            _exitButtonBehaviour.Construct(new DeviceExitButton());
        
        _playButtonBehaviour.Construct(new PlayButton(projectInstaller.SceneLoaderWithCurtains));
        _menuHighScoreController = new MenuHighScoreController(_highScoreView, projectInstaller.ScoreStateContainer);

        _menuEntryPoint.Construct(projectInstaller.SceneLoaderWithCurtains);
    }
}