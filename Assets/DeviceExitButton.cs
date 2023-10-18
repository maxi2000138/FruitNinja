using UnityEngine;

public class DeviceExitButton : IButton
{
    public void OnClick()
    {
        Application.Quit();        
    }
}
