using UnityEngine;

public class LoosePanelView : MonoBehaviour, IPanel, ILateLooseGameListener, IRestartGameListener
{
    public void OnLateLooseGame()
    {
        Show();
    }

    public void OnRestartGame()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}