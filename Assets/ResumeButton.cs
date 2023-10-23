public class ResumeButton : IButton
{
    private readonly PauseController _pauseController;
    private readonly IPanel _pausePanel;

    public ResumeButton(PauseController pauseController, IPanel pausePanel)
    {
        _pauseController = pauseController;
        _pausePanel = pausePanel;
    }
    
    public void OnClick()
    {
        _pausePanel.Hide();
        _pauseController.ResumeGame();        
    }
}