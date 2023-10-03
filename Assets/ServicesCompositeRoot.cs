using UnityEngine;

public class ServicesCompositeRoot : CompositeRoot
{
    [SerializeField] 
    private Camera _camera;
    [SerializeField] 
    private ProjectileShooter _projectileShooter;
    [SerializeField] 
    private Initializer _initializer;

    public override void Compose()
    {
        CameraProvider cameraProvider = new CameraProvider(_camera);
        _projectileShooter.Construct(cameraProvider);
        
        //_initializer.AddInitializable();
    }
}
