using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger
{
    public interface IDestroyTrigger
    {
        void AddDestroyTriggerListeners(ProjectileObject projectileObject);
        void TriggerGroup(ProjectileObject projectileObject, bool isSliced);
        int DestroyGroupCount { get; }
    }
}