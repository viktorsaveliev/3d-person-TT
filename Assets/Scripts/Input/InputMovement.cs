using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Unit))]
public class InputMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Unit _unit;
    private IInputMode _inputMode;

    private Camera _camera;
    private Rigidbody _rigidbody;

    private float _currentSpeed;
    private bool _isAiming;
    private bool _isSprinting;

    private readonly StringBus _stringBus = new();

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _currentSpeed = _unit.Data.RegularSpeed;
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputMode.OnRotated += Rotate;
        _inputMode.OnMove += Move;
        _inputMode.OnAimed += Aim;
        _inputMode.OnSprint += Sprint;
    }

    private void OnDisable()
    {
        _inputMode.OnRotated -= Rotate;
        _inputMode.OnMove -= Move;
        _inputMode.OnAimed -= Aim;
        _inputMode.OnSprint -= Sprint;
    }

    private void Move(Vector3 moveDirection)
    {
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
            _unit.transform.rotation = cameraRotation;

            Vector3 finalMoveDirection = cameraForward * moveDirection.z + _camera.transform.right * moveDirection.x;
            _rigidbody.velocity = finalMoveDirection * _currentSpeed;
            _animator.SetFloat(_stringBus.ANIM_MOVE, _rigidbody.velocity.magnitude);
        }
        else
        {
            _animator.SetFloat(_stringBus.ANIM_MOVE, 0);
        }
    }

    private void Rotate(float mouseX, float mouseY)
    {
        if (!_isAiming) return;

        Vector3 cameraForward = _camera.transform.forward;
        Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
        _unit.transform.rotation = cameraRotation;
    }

    private void Sprint(bool isSprinting)
    {
        if (_isAiming) return;

        _isSprinting = isSprinting;
        _currentSpeed = isSprinting ? _unit.Data.SprintSpeed : _unit.Data.RegularSpeed;
        _animator.SetBool(_stringBus.ANIM_SPRINT, isSprinting);
    }

    private void Aim(bool isAiming)
    {
        if (_isSprinting) return;

        _isAiming = isAiming;
        _currentSpeed = isAiming ? _unit.Data.AimSpeed : _unit.Data.RegularSpeed;

        if (!isAiming)
        {
            Quaternion rotation = Quaternion.Euler(0, _unit.transform.rotation.eulerAngles.y, _unit.transform.rotation.eulerAngles.z);
            _unit.transform.DORotateQuaternion(rotation, 0.5f);
        }

        _animator.SetBool(_stringBus.ANIM_AIM, isAiming);
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
