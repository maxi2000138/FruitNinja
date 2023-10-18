using CodeBase.Infrastructure;

public class PlayButton : IButton
{
    private readonly SceneLoaderWithCurtains _sceneLoaderWithCurtains;

    public PlayButton(SceneLoaderWithCurtains sceneLoaderWithCurtains)
    {
        _sceneLoaderWithCurtains = sceneLoaderWithCurtains;
    }
    
    public void OnClick()
    {
        _sceneLoaderWithCurtains.ShowCurtainsAndLoad("GameScene");
    }
}
