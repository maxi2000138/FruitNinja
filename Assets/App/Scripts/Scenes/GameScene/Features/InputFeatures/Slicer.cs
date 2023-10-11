using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _zPosition;

    private SliceCollidersController _sliceCollidersController;
    private ScreenSettingsProvider _screenSettingsProvider;
    private InputReader _inputReader;
    private Coroutine _sliceCoroutine;
    private Vector2 _lastWorldPosition;
    private bool _isSlicing;

    public void Construct(InputReader inputReader, ScreenSettingsProvider screenSettingsProvider, SliceCollidersController sliceCollidersController)
    {
        _sliceCollidersController = sliceCollidersController;
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
        Vector2 normalizedVector = ((Vector2)worldPosition - _lastWorldPosition).normalized;
        if(normalizedVector == Vector2.zero)
            normalizedVector = Vector2.up;
        
        if (_sliceCollidersController.TryGetIntersectionCollider(worldPosition, out SliceCircleCollider collider))
        {
            collider.SliceBlock.Slice(normalizedVector, 3f);
            collider.Disable();
        }
        
        _trailRenderer.transform.position = worldPosition;
        _lastWorldPosition = worldPosition;
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
