using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelBehaviour : MonoBehaviour
{
    [SerializeField] private List<CustomButton> _buttons;
    
    
    protected void MakeInteractiveButtons()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].MakeInteractable();
        }
    }
    
    protected void MakeNotInteractiveButtons()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].MakeNonInteractive();
        }
    }
    
    protected void DisableAllButonsOnAnyClick()
    {
        AddListenersToAllButtons(DisableAllButons);
    }

    protected void AddListenersToAllButtons(UnityAction listener)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].AddListener(listener);
        }
    }

    private void DisableAllButons()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].MakeNonInteractive();
        }
    }
}
