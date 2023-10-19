using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem
{
    public class ShootSystem : IInitializable, ILooseGameListener, IRestartGameListener
    {
        private readonly IShooter _shooter;
        private readonly IShootPolicy _shootPolicy;

        public ShootSystem(IShooter shooter, IShootPolicy shootPolicy)
        {
            _shooter = shooter;
            _shootPolicy = shootPolicy;
        }

        public void Initialize()
        {
            StartShooting();
        }

        public void OnRestartGame()
        {
            StartShooting();
        }

        public void OnLooseGame()
        {
            StopShooting();
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
}
