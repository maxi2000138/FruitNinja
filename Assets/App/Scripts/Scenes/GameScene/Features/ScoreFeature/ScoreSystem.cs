using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using UnityEngine;

public class ScoreSystem : IRestartGameListener, IDestroyable
{
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    
    private readonly ScoreView _currentScoreView;
    private readonly ScoreView _highScoreView;
    private readonly SaveDataContainer<ScoreData> _scoreContainer;
    private readonly ScoreConfig _scoreConfig;
    private readonly Slicer _slicer;

    private int _previousScore = 0;
    private int _previousHighScore = 0;

    public ScoreSystem(SaveDataContainer<ScoreData> scoreContainer, Slicer slicer, ScoreView currentScoreView, ScoreView highScoreView, ScoreConfig scoreConfig)
    {
        _scoreConfig = scoreConfig;
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

    public void OnRestartGame()
    {
        CurrentScore = 0;
        ResetScores();
    }

    private void OnSlice(Vector2 projectilePosition, ProjectileType projectileType)
    {
        if(projectileType != ProjectileType.Fruit)
            return;

        AddSliceScore(_scoreConfig.SliceScore);
    }

    public void AddSliceScore(int currentScore)
    {
        _previousScore = CurrentScore;
        CurrentScore += currentScore;

        if (HighScore < CurrentScore)
        {
            _previousHighScore = HighScore;
            HighScore = CurrentScore;
            _scoreContainer.WriteData().HighScore = HighScore;
        }

        UpdateScores();
    }

    private void OnScoreDataLoaded()
    {
        ScoreData scoreData = _scoreContainer.ReadData();
        HighScore = scoreData.HighScore;
        _previousHighScore = HighScore;
        UpdateScores();
    }

    private void UpdateScores()
    {
        _currentScoreView.UpdateText(_previousScore, CurrentScore);
        _highScoreView.UpdateText(_previousHighScore,HighScore);
    }
    
    private void ResetScores()
    {
        _currentScoreView.ResetText(CurrentScore);
        _highScoreView.ResetText(HighScore);
    }
}
