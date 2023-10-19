using System.Collections.Generic;
using UnityEngine;

public class GameStateObserver
{
    private readonly List<object> _gameListeners = new();
    private readonly List<object> _loosePolicies = new();


    public void AddPolicy(object policy)
    {
        _loosePolicies.Add(policy);

        if (policy is ILoosePolicy loosePolicy)
        {
            loosePolicy.NeedLoose += LooseGame;
            loosePolicy.NeedLateLoose += LateLooseGame;
        }

        if (policy is IRestartPolicy restartPolicy)
            restartPolicy.NeedRestart += RestartGame;
    }

    public void RemovePolicy(object policy)
    {
        _loosePolicies.Remove(policy);

        if (policy is ILoosePolicy loosePolicy)
        {
            loosePolicy.NeedLoose -= LooseGame;
            loosePolicy.NeedLateLoose -= LateLooseGame;
        }
        
        if (policy is IRestartPolicy restartPolicy)
            restartPolicy.NeedRestart -= RestartGame;
    }
    
    public void AddObserver(object listener)
    {
        _gameListeners.Add(listener);
    }
    
    public void RemoveObserver(object listener)
    {
        _gameListeners.Remove(listener);
    }


    private void RestartGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is IRestartGameListener pauseGameListener)
                pauseGameListener.OnRestartGame();
        }
    }
    
    private void LooseGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is ILooseGameListener pauseGameListener)
                pauseGameListener.OnLooseGame();
        }
        
        Debug.Log("Loose");
    }
    
    private void LateLooseGame()
    {
        foreach (object listener in _gameListeners)
        {
            if(listener is ILateLooseGameListener pauseGameListener)
                pauseGameListener.OnLateLooseGame();
        }
        
        Debug.Log("Late Loose");
    }
}