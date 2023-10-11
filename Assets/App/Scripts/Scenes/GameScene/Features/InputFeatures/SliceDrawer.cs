using UnityEngine;

public class SliceDrawer : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _zPosition;

    private ScreenSettingsProvider _screenSettingsProvider;
    private ICoroutineRunner _coroutineRunner;
    private InputReader _inputReader;
    private Coroutine _sliceCoroutine;
    private bool _isSlicing;

    public void Construct(InputReader inputReader, ScreenSettingsProvider screenSettingsProvider, ICoroutineRunner coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
        _screenSettingsProvider = screenSettingsProvider;
        _inputReader = inputReader;
        Enable();
    }

    private void Update()
    {
        if(!_isSlicing)
            return;
        
        Vector3 worldPosition = _screenSettingsProvider.ScreenToWorldPosition(_inputReader.TouchPosition);
        worldPosition.z = _zPosition;
        _trailRenderer.transform.position = worldPosition;
    }

    private void OnDestroy()
    {
        Disable();
    }

    private void StartSlicing()
    {
        _trailRenderer.Clear();
        _trailRenderer.enabled = true;
        _isSlicing = true;
    }

    private void EndSlicing()
    {
        _isSlicing = false;
        _trailRenderer.enabled = false;
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
}
