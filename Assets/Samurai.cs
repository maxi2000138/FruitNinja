using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem;
using UnityEngine;

public class Samurai : MonoBehaviour, ISlicable
{
    private SamuraiController _samuraiController;
    private ShootSystem _shootSystem;

    public void Construct(SamuraiController samuraiController)
    {
        _samuraiController = samuraiController;
    }
    
    public void OnSlice()
    {
        _samuraiController.StartSamurai();
    }
}
