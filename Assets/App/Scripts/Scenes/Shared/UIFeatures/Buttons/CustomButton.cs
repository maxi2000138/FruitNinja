using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private IButton _buttonBehaviour;
    private TweenCore _tweenCore;
    private TokenController _tokenController;

    public void Construct(IButton buttonBehaviour, TweenCore tweenCore)
    {
        _tokenController = new TokenController();
        _tweenCore = tweenCore;
        _buttonBehaviour = buttonBehaviour;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ScaleOnClick);
        _button.onClick.AddListener(_buttonBehaviour.OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
        _tokenController.CancelTokens();
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

    private void ScaleOnClick()
    {
        _tweenCore.PunchByTime((scale) => _button.transform.localScale = scale, _button.transform.localScale,
            _button.transform.localScale * 0.9f, 0.2f, CustomEase.FullCosine, _tokenController.CreateCancellationToken());
    }
}
