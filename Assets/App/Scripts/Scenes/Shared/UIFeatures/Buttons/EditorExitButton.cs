public class EditorExitButton : IButton
{
    public void OnClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
