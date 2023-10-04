using UnityEngine;

public class ServicesCompositeRoot : CompositeRoot
{
    [SerializeField] 
    private Camera _camera;
    [SerializeField] 
    private SpawnAreasSetuper _spawnAreasSetuper;
    [SerializeField] 
    private MonoBehaviourSimulator _namemonoBehaviourSimulator;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        CameraFeaturesProvider cameraFeaturesProvider = new CameraFeaturesProvider(_camera);
        ProjectileFactory projectileFactory = new ProjectileFactory();
        ProjectileShooter projectileShooter = new ProjectileShooter(projectileFactory, _spawnAreasSetuper, cameraFeaturesProvider);
        monoBehaviourSimulator.AddUpdatable(projectileShooter);
    }
}
