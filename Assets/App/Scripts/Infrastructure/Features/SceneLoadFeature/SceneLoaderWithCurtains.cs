using System;
using CodeBase.Infrastructure;
using CodeBase.Logic;

public class SceneLoaderWithCurtains
{
    private readonly LoadingCurtain _loadingCurtain;
    private readonly SceneLoader _sceneLoader;

    public SceneLoaderWithCurtains(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
    }

    public void ShowCurtainsAndLoad(string name, Action onLoaded = null)
    {
        _loadingCurtain.Show();
        _loadingCurtain.OnHide += () => _sceneLoader.Load(name, onLoaded);
    }

    public void HideCurtains()
    {
        _loadingCurtain.Hide();
    }
}
