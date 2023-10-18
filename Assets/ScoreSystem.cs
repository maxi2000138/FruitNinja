using App.Scripts.Scenes.GameScene.Features.InputFeatures;

public class ScoreSystem : IDestroyable
{
    private readonly ScoreView _currentScoreView;
    private readonly ScoreView _highScoreView;
    private readonly SaveDataContainer<ScoreData> _scoreContainer;
    private readonly Slicer _slicer;

    private int _currentScore = 0;
    private int _highScore = 0;

    public ScoreSystem(SaveDataContainer<ScoreData> scoreContainer, Slicer slicer, ScoreView currentScoreView, ScoreView highScoreView)
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
        ScoreData scoreData = _scoreContainer.ReadData();
        _highScore = scoreData.HighScore;
        UpdateScores();
    }

    private void UpdateScores()
    {
        _currentScoreView.UpdateText(_currentScore.ToString());
        _highScoreView.UpdateText("Лучший: " + _highScore);
    }
}
