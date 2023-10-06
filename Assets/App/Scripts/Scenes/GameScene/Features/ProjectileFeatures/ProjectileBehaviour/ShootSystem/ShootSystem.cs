using UnityEngine;

public class ShootSystem
{
    private readonly IShooter _shooter;
    private readonly IShootPolicy _shootPolicy;

    public ShootSystem(IShooter shooter, IShootPolicy shootPolicy)
    {
        _shooter = shooter;
        _shootPolicy = shootPolicy;
    }

    public void StartShooting()
    {
        StopShooting();
        _shootPolicy.NeedShoot += _shooter.Shoot;
        _shootPolicy.StartWorking();
    }

    public void StopShooting()
    {
        _shootPolicy.StopWorking();
        _shootPolicy.NeedShoot -= _shooter.Shoot;
    }
}
