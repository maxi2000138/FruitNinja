using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private IButton _buttonBehaviour;

    public void Construct(IButton buttonBehaviour)
    {
        _buttonBehaviour = buttonBehaviour;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(_buttonBehaviour.OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void AddListener(UnityAction listener)
    {
        _button.onClick.AddListener(listener);
    }
    
    public void MakeInteractable()
    {
        _button.interactable = true;
    }

    public void MakeNonInteractive()
    {
        _button.interactable = false;
    }
}
