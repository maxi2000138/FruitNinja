using System.Threading.Tasks;
using UnityEngine;

public class AlphaTweener
{
    private int _deltaTime = 10;
    
    public async Task TweenAlpha(CanvasGroup canvasGroup, byte startValue, byte endValue, float time)
    {
        float startValue01 = (float)startValue / 256;
        float endValue01 = (float)endValue / 256;
        float currentTime = 0f;
        float startTime;

        while (currentTime < time)
        {
            canvasGroup.alpha = easeOutQuad(Mathf.Lerp(startValue01, endValue01, currentTime/time));
            startTime = Time.time;
            await Task.Delay(_deltaTime);
            currentTime += Time.time - startTime;
        }
    }
    
    private float easeOutQuad(float x) 
    {
        return 1 - (1 - x) * (1 - x);
    }
}
