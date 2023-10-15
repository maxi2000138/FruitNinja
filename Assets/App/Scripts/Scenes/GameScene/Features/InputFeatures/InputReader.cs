using System;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.InputFeatures
{
    public class InputReader : IUpdatable
    {
        public event Action SliceStartedEvent; 
        public event Action SliceEndedEvent;
        public Vector2 TouchPosition { get; private set; }

       
        public void Update(float deltaTime)
        {
            if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                SliceStartedEvent?.Invoke();
            }
            
            if(Input.GetMouseButton(0))
            {
                TouchPosition = Input.mousePosition;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                TouchPosition = Input.GetTouch(0).position;
            }
            
            if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled))
            {
                SliceEndedEvent?.Invoke();
            }
        }
    }
}
