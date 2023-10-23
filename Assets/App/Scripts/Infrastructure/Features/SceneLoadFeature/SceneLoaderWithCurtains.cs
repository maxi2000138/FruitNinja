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

    public async void ShowCurtainsAndLoad(string name, Action onLoaded = null)
    {
        await _loadingCurtain.Show();
        await _sceneLoader.Load(name, onLoaded);
    }

    public void HideCurtains()
    {
        _loadingCurtain.Hide();
    }
}
