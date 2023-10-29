using UnityEngine;

public class FreezeView : MonoBehaviour, IPanel
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
