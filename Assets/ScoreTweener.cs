using System;

public class ScoreTweener
{
    private readonly TweenCore _tweenCore;
    private Action<int> _setScoreAction;

    public ScoreTweener(TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
    }
    
    public void TweenScore(Action<int> setScoreAction, int startValue, int endValue, float time, CustomEase ease)
    {
        _setScoreAction = setScoreAction;
        //_tweenCore.TweenByTime(SetScore, startValue, endValue, time, ease);
    }
    
    private void SetScore(float value)
    {
        _setScoreAction?.Invoke((int)value);
    }
}



