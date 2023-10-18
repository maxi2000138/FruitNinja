using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LoadingCurtain : MonoBehaviour
  {
    private const float AlphaChangeDelta = 0.03f;
    private const float HideSpeed = 0.2f;
    private const float FadeSpeed = 0.03f;
    public event Action OnHide;

    public CanvasGroup Curtain;
    

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void Show()
    {
      gameObject.SetActive(true);
      StartCoroutine(DoHideIn());
    }

    public void Hide() => StartCoroutine(DoFadeIn());

    private IEnumerator DoFadeIn()
    {
      while (Curtain.alpha > 0)
      {
        Curtain.alpha -= FadeSpeed;
        yield return new WaitForSeconds(AlphaChangeDelta);
      }

      gameObject.SetActive(false);
    }

    private IEnumerator DoHideIn()
    {
      while (Curtain.alpha < 1)
      {
        Curtain.alpha += HideSpeed;
        yield return new WaitForSeconds(AlphaChangeDelta);
      }

      OnHide?.Invoke();
    }

  }
}
