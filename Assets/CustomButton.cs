using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private IButton _buttonBehaviour;

    public void Construct(IButton buttonBehaviour)
    {
        _buttonBehaviour = buttonBehaviour;
        _button.onClick.AddListener(_buttonBehaviour.OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(_buttonBehaviour.OnClick);
    }
}
