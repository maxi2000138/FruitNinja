using System;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public interface IShootPolicy
    {
        public event Action NeedShoot;
        public void StartWorking();
        public void StopWorking();
    }
}
