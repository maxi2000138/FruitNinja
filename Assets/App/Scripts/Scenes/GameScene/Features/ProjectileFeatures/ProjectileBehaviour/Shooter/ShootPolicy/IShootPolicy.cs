using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy
{
    public interface IShootPolicy
    {
        public event Action NeedShoot;
        public void StartWorking();
        public void StopWorking();
    }
}
