using System;

public class RestartGameButton : IButton, IRestartPolicy
{
    public event Action NeedRestart;
    
    private readonly SceneLoaderWithCurtains _sceneLoaderWithCurtains;

    public RestartGameButton(SceneLoaderWithCurtains sceneLoaderWithCurtains)
    {
        _sceneLoaderWithCurtains = sceneLoaderWithCurtains;
    }
    public void OnClick()
    {
        _sceneLoaderWithCurtains.ShowCurtainsAndLoad("GameScene", OnLoaded);
    }

    private void OnLoaded()
    {
        _sceneLoaderWithCurtains.HideCurtains();
        NeedRestart?.Invoke();
    }
}
