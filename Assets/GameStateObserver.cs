using System.Collections.Generic;

public class GameStateObserver
{
    private readonly List<object> _gameListeners = new();

    public void AddObserver(object listener)
    {
        _gameListeners.Add(listener);
    }
    
    public void RemoveObserver(object listener)
    {
        _gameListeners.Remove(listener);
    }


    public void PauseGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is IPauseGameListener pauseGameListener)
                pauseGameListener.OnPauseGame();
        }
    }
    
    public void ResumeGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is IResumeGameListener pauseGameListener)
                pauseGameListener.OnResumeGame();
        }
    }
    
    public void WinGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is IWinGameListener pauseGameListener)
                pauseGameListener.OnWinGame();
        }
    }
    
    public void LooseGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is ILooseGameListener pauseGameListener)
                pauseGameListener.OnLooseGame();
        }
    }
}