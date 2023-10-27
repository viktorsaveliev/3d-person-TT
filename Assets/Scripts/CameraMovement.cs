using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;

    private IInputMode _inputMode;

    private readonly Vector3 _regularOffset = new(0.72f, 0.2f, -2);
    private readonly Vector3 _aimOffset = new (0.5f, 0.4f, -0.5f);

    private Vector3 _currentOffset;

    // === для плавного перехода
    private Vector3 _startOffset; 

    private readonly float _transitionDuration = 0.2f;
    private float _transitionTime = 0f;
    // ===

    private Vector3 _shakeOffset;
    private readonly float _shakeSensitivity = 0.008f;

    private readonly float _sensitivity = 3;
    private readonly float _limitRotateX = 25;
    private float _rotationX, _rotationY;

    private bool _isAiming;

    private bool IsRotateActive => Cursor.lockState == CursorLockMode.Locked;

    private void Awake()
    {
        _currentOffset = _regularOffset;
        _transitionTime = _transitionDuration;
        transform.position = _cameraTarget.position + _currentOffset;
    }

    private void OnEnable()
    {
        _inputMode.OnRotated += Rotate;
        _inputMode.OnAimed += Aim;
        _inputMode.OnShot += OnPlayerShot;
    }

    private void OnDisable()
    {
        _inputMode.OnRotated -= Rotate;
        _inputMode.OnAimed -= Aim;
        _inputMode.OnShot -= OnPlayerShot;
    }

    private void Rotate(float mouseX, float mouseY)
    {
        if (!IsRotateActive) return;

        _rotationX = transform.localEulerAngles.y + mouseX * _sensitivity;
        _rotationY += mouseY * _sensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -_limitRotateX, _limitRotateX);

        if (_transitionTime < _transitionDuration)
        {
            _currentOffset = Vector3.Lerp(_startOffset, _isAiming ? _aimOffset : _regularOffset, _transitionTime / _transitionDuration);
            _transitionTime += Time.deltaTime;
        }

        if (_isAiming)
        {
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0) + _shakeOffset;
            transform.position = transform.localRotation * _currentOffset + _cameraTarget.position + _shakeOffset;
            _shakeOffset = Vector3.zero;
        }
        else
        {
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
            transform.position = transform.localRotation * _currentOffset + _cameraTarget.position;
        }
    }

    private void Aim(bool isAiming)
    {
        _startOffset = _currentOffset;
        _transitionTime = 0f;

        _isAiming = isAiming;
        _currentOffset = isAiming ? _aimOffset : _regularOffset;
    }

    private void OnPlayerShot()
    {
        _shakeOffset = new(
            Random.Range(-_shakeSensitivity, _shakeSensitivity), 
            Random.Range(-_shakeSensitivity, _shakeSensitivity), 
            0
        );
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
