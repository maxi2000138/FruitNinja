using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _button;
    
    private IButton _buttonBehaviour;
    private TweenCore _tweenCore;
    private TokenController _tokenController;
    private Vector3 _buttonScale;


    public void Construct(IButton buttonBehaviour, TweenCore tweenCore)
    {
         _buttonScale = _button.transform.localScale;
        _tokenController = new TokenController();
        _tweenCore = tweenCore;
        _buttonBehaviour = buttonBehaviour;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(_buttonBehaviour.OnClick);
    }
    
    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
        _tokenController.CancelTokens();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _tweenCore.TweenByTime(scale => _button.transform.localScale = scale, _button.transform.localScale,
            _buttonScale * 0.9f, 0.1f, CustomEase.Linear, _tokenController.CreateCancellationToken());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _tweenCore.TweenByTime(scale => _button.transform.localScale = scale, _button.transform.localScale,
            _buttonScale, 0.1f, CustomEase.Linear, _tokenController.CreateCancellationToken());
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
