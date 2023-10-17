using UnityEngine;
using Zenject;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;

    private IInputMode _inputMode;

    private readonly Vector3 _regularOffset = new(0.72f, 0.2f, -2);
    private readonly Vector3 _aimOffset = new (0.5f, 0.4f, -0.5f);

    private Vector3 _currentOffset;

    private readonly float _sensitivity = 3;
    private readonly float _limitRotateX = 25;
    private float _rotationX, _rotationY;

    private void Awake()
    {
        _currentOffset = _regularOffset;
        transform.position = _cameraTarget.position + _currentOffset;
    }

    private void OnEnable()
    {
        _inputMode.OnRotated += Rotate;
        _inputMode.OnAimed += Aim;
    }

    private void OnDisable()
    {
        _inputMode.OnRotated -= Rotate;
        _inputMode.OnAimed -= Aim;
    }

    private void Rotate(float mouseX, float mouseY)
    {
        _rotationX = transform.localEulerAngles.y + mouseX * _sensitivity;
        _rotationY += mouseY * _sensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -_limitRotateX, _limitRotateX);

        transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        transform.position = transform.localRotation * _currentOffset + _cameraTarget.position;
    }

    private void Aim(bool isAiming)
    {
        _currentOffset = isAiming? _aimOffset : _regularOffset;
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
