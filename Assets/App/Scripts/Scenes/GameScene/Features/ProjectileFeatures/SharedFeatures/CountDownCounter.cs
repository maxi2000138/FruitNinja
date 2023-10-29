using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CountDownCounter
{
    private float startTime;
    private float _currentSeconds = 0f;
    private int _timerMlsDeltaTime = 40;
    private CountDownView _countDownView;
    private readonly TokenController _tokenController;
    private CancellationToken _currentToken;


    public CountDownCounter(CountDownView countDownView)
    {
        _countDownView = countDownView;
        _tokenController = new TokenController();
    }
    
    public async UniTaskVoid StartCountdown(float seconds)
    {
        if (_currentToken != default)
        {
            _tokenController.CancelToken(_currentToken);
            _currentToken = default;
        }

        _currentSeconds = seconds;
        
        while (_currentSeconds > 0)
        {
            _countDownView.SetText(((int)_currentSeconds+1).ToString());
            startTime = Time.realtimeSinceStartup;
            _currentToken = _tokenController.CreateCancellationToken();
            bool canceled = await UniTask.Delay(_timerMlsDeltaTime, DelayType.DeltaTime, PlayerLoopTiming.Update,
                _currentToken).SuppressCancellationThrow();
            if(canceled)
                return;
            _currentSeconds -= Time.realtimeSinceStartup - startTime;
        }
    }
}