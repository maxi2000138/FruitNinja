using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger
{
    public interface IDestroyTrigger
    {
        void AddDestroyTriggerListeners(params Transform[] objectTransform);
        void TriggerGroup(Transform destroyTransform);
    }
}