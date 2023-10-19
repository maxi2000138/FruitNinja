using System;
using UnityEngine;

public class MenuHighScoreController : MonoBehaviour, IDestroyable
{
    private ScoreView _scoreView;
    private readonly SaveDataContainer<ScoreData> _scoreDataContainer;

    public MenuHighScoreController(ScoreView scoreView, SaveDataContainer<ScoreData> scoreDataContainer)
    {
        _scoreView = scoreView;
        _scoreDataContainer = scoreDataContainer;

        _scoreDataContainer.DataLoaded += SetScore;
        SetScore();
    }

    public void OnDestroy()
    {
        _scoreDataContainer.DataLoaded -= SetScore;
    }

    private void SetScore()
    {
        _scoreView.UpdateText(_scoreDataContainer.ReadData().HighScore.ToString());
    }
}
