namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.EntryPoint
{
    public class GameEntryPoint : EntryPointBehaviour
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
}
