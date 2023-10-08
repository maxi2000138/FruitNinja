using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : InputActions.IGameSceneActions
{
    public event Action SliceStartedEvent; 
    public event Action SliceEndedEvent; 
    
    public Vector2 TouchPosition { get; private set; }

    private readonly InputActions _inputActions;
    
    public InputReader()
    {
        _inputActions = new InputActions();
        _inputActions.GameScene.SetCallbacks(this);
        _inputActions.Enable();
    }
    
    public void OnSlice(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SliceStartedEvent?.Invoke();
        }
        else if (context.canceled)
        {
            SliceEndedEvent?.Invoke();
        }
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        TouchPosition = context.ReadValue<Vector2>();
    }
}
