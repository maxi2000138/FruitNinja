public class MenuEntryPoint : EntryPointBehaviour
{
    private SceneLoaderWithCurtains _sceneLoaderWithCurtains;

    public void Construct(SceneLoaderWithCurtains sceneLoaderWithCurtains)
    {
        _sceneLoaderWithCurtains = sceneLoaderWithCurtains;
    }
    
    public override void OnEntryPoint()
    {
        _sceneLoaderWithCurtains.HideCurtains();   
    }
}
