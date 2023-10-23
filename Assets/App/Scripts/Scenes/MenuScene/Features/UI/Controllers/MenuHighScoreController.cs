using UnityEngine;

public class MenuHighScoreController : MonoBehaviour, IDestroyable
{
    private SetupTextView _scoreView;
    private int _lastScore;
    private SaveDataContainer<ScoreData> _scoreDataContainer;

    public void Construct(SetupTextView scoreView, SaveDataContainer<ScoreData> scoreDataContainer)
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
        _scoreView.SetupText(_scoreDataContainer.ReadData().HighScore.ToString());
    }
}
