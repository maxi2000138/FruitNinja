using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SliceDrawer : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _zPosition;

    private ScreenSettingsProvider _screenSettingsProvider;
    private ICoroutineRunner _coroutineRunner;
    private InputReader _inputReader;
    private Coroutine _sliceCoroutine;

    public void Construct(InputReader inputReader, ScreenSettingsProvider screenSettingsProvider, ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
        _screenSettingsProvider = screenSettingsProvider;
        _inputReader = inputReader;
        Enable();
    }

    private void OnDestroy()
    {
        Disable();
    }

    private void StartSlicing()
    {
        _sliceCoroutine = _coroutineRunner.StartCoroutine(SliceCoroutine());
        _trailRenderer.Clear();
        _trailRenderer.enabled = true;
    }

    private void EndSlicing()
    {
        _trailRenderer.enabled = false;
        if(_sliceCoroutine != null)
            _coroutineRunner.StopCoroutine(_sliceCoroutine);
    }

    private void Enable()
    {
        _inputReader.SliceStartedEvent += StartSlicing;
        _inputReader.SliceEndedEvent += EndSlicing;
    }

    private void Disable()
    {
        _inputReader.SliceStartedEvent -= StartSlicing;
        _inputReader.SliceEndedEvent -= EndSlicing;
    }

    private IEnumerator SliceCoroutine()
    {
        while (true)
        {
            Vector3 worldPosition = _screenSettingsProvider.ScreenToWorldPosition(_inputReader.TouchPosition);
            worldPosition.z = _zPosition;
            _trailRenderer.transform.position = worldPosition;
            yield return null;
        }
    }
}
