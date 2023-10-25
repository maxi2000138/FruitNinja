using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using UnityEngine;

public class Brick : MonoBehaviour, ISlicable
{
    private Slicer _slicer;

    public void Construct(Slicer slicer)
    {
        _slicer = slicer;
    }
    
    public void OnSlice()
    {
        
    }
}
