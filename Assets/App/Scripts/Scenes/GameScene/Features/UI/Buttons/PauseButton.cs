public class PauseButton : IButton
{
    private readonly PauseController _pauseController;
    private readonly IPanel _pausePanel;

    public PauseButton(PauseController pauseController, IPanel pausePanel)
    {
        _pauseController = pauseController;
        _pausePanel = pausePanel;
    }
    
    public void OnClick()
    {
        _pausePanel.Show();
        _pauseController.PauseGame();        
    }
}
