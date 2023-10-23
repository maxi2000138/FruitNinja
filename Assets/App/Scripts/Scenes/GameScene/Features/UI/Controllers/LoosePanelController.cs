public class LoosePanelController : ILateLooseGameListener, IRestartGameListener
{
    private readonly LoosePanelView _loosePanelView;
    private readonly ScoreSystem _scoreSystem;

    public LoosePanelController(LoosePanelView loosePanelView, ScoreSystem scoreSystem)
    {
        _loosePanelView = loosePanelView;
        _scoreSystem = scoreSystem;
    }
    public void OnLateLooseGame()
    {
        _loosePanelView.Setup(_scoreSystem.CurrentScore, _scoreSystem.HighScore);
        _loosePanelView.Show();
    }

    public void OnRestartGame()
    {
        _loosePanelView.Hide();
    }
}
