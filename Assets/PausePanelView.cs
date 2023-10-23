using UnityEngine;

public class PausePanelView : MonoBehaviour, IPanel
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
