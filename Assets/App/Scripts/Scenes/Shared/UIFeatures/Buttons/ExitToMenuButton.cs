using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitToMenuButton : IButton
{
    private readonly SceneLoaderWithCurtains _sceneLoaderWithCurtains;

    public ExitToMenuButton(SceneLoaderWithCurtains sceneLoaderWithCurtains)
    {
        _sceneLoaderWithCurtains = sceneLoaderWithCurtains;
    }
    public void OnClick()
    {
        Time.timeScale = 1f;
        _sceneLoaderWithCurtains.ShowCurtainsAndLoad("MenuScene");
    }
}
