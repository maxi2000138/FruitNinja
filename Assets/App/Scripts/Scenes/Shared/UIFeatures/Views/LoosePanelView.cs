using TMPro;
using UnityEngine;

public class LoosePanelView : PanelBehaviour
{
    [SerializeField] private CanvasGroup _canvasInfoGroup;
    [SerializeField] private CanvasGroup _PopupGroup;
    [SerializeField] private CanvasGroup _scoreGroup;
    [SerializeField] private TMP_Text _resultValueText;
    [SerializeField] private TMP_Text _highScoreValueText;

    private float _currentScore;
    private float _highScore;
    private TweenCore _tweenCore;
    private TokenController _tokenController;
    private Vector3 _transformLocalScale;

    public void Construct(TweenCore tweenCore)
    {
        _tweenCore = tweenCore;
        _tokenController = new TokenController();
    }

    public void Setup(float currentScore, float highScore)
    {
        _highScore = highScore;
        _currentScore = currentScore;
        _transformLocalScale = _resultValueText.transform.localScale;
    }

    private void OnDestroy()
    {
        _tokenController.CancelTokens();
    }

    public async void Show()
    {
        MakeNotInteractiveButtons();
        DisableAllButonsOnAnyClick();
        _resultValueText.text = _currentScore.ToString();
        _highScoreValueText.text = "Лучший: " + _highScore;
        gameObject.SetActive(true);

        _PopupGroup.alpha = 0f;
        _canvasInfoGroup.alpha = 0f;
        _scoreGroup.alpha = 0f;
        await _tweenCore.TweenByTime(SetCanvasPopupAlpha, 0f, 1f, 0.8f, CustomEase.OutQuad, _tokenController.CreateCancellationToken());
        MakeInteractiveButtons();
        await _tweenCore.TweenByTime(SetCanvasInfoALpha, 0f, 1f, 0.6f, CustomEase.OutQuad, _tokenController.CreateCancellationToken());
        _tweenCore.TweenByTime(SetCanvasScoreAlpha, 0f, 1f, 1f, CustomEase.OutQuad, _tokenController.CreateCancellationToken());

        if (_currentScore != 0)
            await _tweenCore.TweenByTime(SetScoreText, 0, _currentScore, 1f, CustomEase.OutQuad, _tokenController.CreateCancellationToken());

        _tweenCore.PunchByTime(SetTextScale, _transformLocalScale,
            _transformLocalScale * 1.5f, 0.5f, CustomEase.FullCosine, _tokenController.CreateCancellationToken());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    private void SetCanvasInfoALpha(float alpha) =>
        _canvasInfoGroup.alpha = alpha;

    private void SetCanvasPopupAlpha(float alpha) =>
        _PopupGroup.alpha = alpha;

    private void SetCanvasScoreAlpha(float alpha) =>
        _scoreGroup.alpha = alpha;

    private void SetScoreText(float text) =>
        _resultValueText.text = ((int)text).ToString();

    private void SetTextScale(Vector3 value) =>
        _resultValueText.transform.localScale = value;

}