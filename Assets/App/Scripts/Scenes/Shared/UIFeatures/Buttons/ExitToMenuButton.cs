public class ExitToMenuButton : IButton
{
    private readonly SceneLoaderWithCurtains _sceneLoaderWithCurtains;

    public ExitToMenuButton(SceneLoaderWithCurtains sceneLoaderWithCurtains)
    {
        _sceneLoaderWithCurtains = sceneLoaderWithCurtains;
    }
    public void OnClick()
    {
        _sceneLoaderWithCurtains.ShowCurtainsAndLoad("MenuScene");
    }
}
