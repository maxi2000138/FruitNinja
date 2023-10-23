using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LoadingCurtain : MonoBehaviour
  {
    public CanvasGroup Curtain;

    private TweenCore _tweenCore;


    public void Construct(TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public async Task Show()
    {
      gameObject.SetActive(true);
      await _tweenCore.TweenByTime(SetAlpha, 0f, 1f, 0.6f, CustomEase.Linear, new CancellationToken());
    }

    private void SetAlpha(float value)
    {
      Curtain.alpha = value;
    }

    public async Task Hide()
    {
      await _tweenCore.TweenByTime(SetAlpha, 1f, 0f, 0.8f, CustomEase.Linear, new CancellationToken());
      gameObject.SetActive(false);
    }
    
  }
}
