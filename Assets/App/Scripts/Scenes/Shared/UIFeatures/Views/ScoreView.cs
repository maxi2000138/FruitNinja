using System.Threading;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour 
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _preText;
    
    private TweenCore _tweenCore;
    private CancellationTokenSource _cts;

    public void Construct(TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
    }

    public void ResetText(float value)
    {
        SetText(value);
    }
    
    public void UpdateText(int startScore, int newScore)
    {
        if(_cts != null)
            _cts.Cancel();

        _cts = new CancellationTokenSource();
        _tweenCore.TweenByTime(SetText, startScore, newScore, 1f, CustomEase.OutQuad, _cts.Token);
    }

    private void SetText(float value)
    {
        _text.text = _preText + ((int)value);
    }
}
