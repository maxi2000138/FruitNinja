using App.Scripts.Scenes.GameScene.Features.InputFeatures;

public class ScoreSystem : IDestroyable
{
    private readonly ScoreView _currentScoreView;
    private readonly ScoreView _highScoreView;
    private readonly SaveDataContainer<ScoreState> _scoreContainer;
    private readonly Slicer _slicer;

    private int _currentScore = 0;
    private int _highScore = 0;

    public ScoreSystem(SaveDataContainer<ScoreState> scoreContainer, Slicer slicer, ScoreView currentScoreView, ScoreView highScoreView)
    {
        _currentScoreView = currentScoreView;
        _highScoreView = highScoreView;
        _scoreContainer = scoreContainer;
        _slicer = slicer;

        _scoreContainer.DataLoaded += OnScoreDataLoaded;
        _slicer.OnSlice += OnSlice;
        
        OnScoreDataLoaded();
        UpdateScores();
    }

    public void OnDestroy()
    {
        _slicer.OnSlice -= OnSlice;
        _scoreContainer.DataLoaded -= OnScoreDataLoaded;
    }

    private void OnSlice()
    {
        _currentScore += 20;
        
        if (_highScore < _currentScore)
        {
            _highScore = _currentScore;
            _scoreContainer.WriteData().HighScore = _highScore;
        }
        
        UpdateScores();
    }

    private void OnScoreDataLoaded()
    {
        ScoreState scoreState = _scoreContainer.ReadData();
        _highScore = scoreState.HighScore;
        UpdateScores();
    }

    private void UpdateScores()
    {
        _currentScoreView.UpdateText("Score: " + _currentScore);
        _highScoreView.UpdateText("High Score: " + _highScore);
    }
}
