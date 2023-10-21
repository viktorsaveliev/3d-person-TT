using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Unit))]
public class InputMovement : MonoBehaviour
{
    [SerializeField] private Transform _unitTransform;

    private UserCharacter _unit;
    private IInputMode _inputMode;

    private Camera _camera;
    private Rigidbody _rigidbody;

    private float _currentSpeed;
    private bool _isAiming;
    private bool _isGrounded;
    private bool _isSprinting;

    private readonly StringBus _stringBus = new();
    private bool IsMoveActive => !Cursor.visible;

    private void Awake()
    {
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
        _inputMode.OnJump += Jump;
    }

    private void OnDisable()
    {
        _inputMode.OnRotated -= Rotate;
        _inputMode.OnMove -= Move;
        _inputMode.OnAimed -= Aim;
        _inputMode.OnSprint -= Sprint;
        _inputMode.OnJump -= Jump;
    }

    private void Move(Vector3 moveDirection)
    {
        if (!IsMoveActive || !_isGrounded) return;

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Vector3 cameraForward = _camera.transform.forward;
            cameraForward.y = 0;

            Vector3 finalMoveDirection = cameraForward * moveDirection.z + _camera.transform.right * moveDirection.x;

            if (!_isAiming)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(finalMoveDirection);
                _unitTransform.rotation = desiredRotation;
            }

            _rigidbody.velocity = finalMoveDirection.normalized * _currentSpeed;
            _unit.Animator.SetFloat(_stringBus.ANIM_MOVE, _rigidbody.velocity.magnitude);
        }
        else
        {
            _unit.Animator.SetFloat(_stringBus.ANIM_MOVE, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        int groundLayer = 3;
        if (collision.gameObject.layer == groundLayer)
        {
            _isGrounded = true;
        }
    }

    private void Jump()
    {
        if (!_isGrounded) return;

        float jumpForce = 100f;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _isGrounded = false;

        _unit.Animator.SetTrigger(_stringBus.ANIM_JUMP);
    }

    private void Rotate(float mouseX, float mouseY)
    {
        if (!_isAiming || !_isGrounded) return;

        _unitTransform.rotation = GetCameraLookRotation();
    }

    private void Sprint(bool isSprinting)
    {
        if (_isAiming || !_isGrounded) return;

        if (isSprinting)
        {
            if (_unit.IsReloading) return;
            _currentSpeed = _unit.Data.SprintSpeed;
        }
        else
        {
            _currentSpeed = _unit.Data.RegularSpeed;
        }

        _isSprinting = isSprinting;
        _unit.Animator.SetBool(_stringBus.ANIM_SPRINT, isSprinting);
    }

    private void Aim(bool isAiming)
    {
        if (!IsMoveActive || !_isGrounded) return;

        _isAiming = isAiming;
        _currentSpeed = isAiming ? _unit.Data.AimSpeed : _unit.Data.RegularSpeed;

        if (!isAiming)
        {
            Quaternion rotation = Quaternion.Euler(0, _unitTransform.rotation.eulerAngles.y, _unitTransform.rotation.eulerAngles.z);
            _unitTransform.DORotateQuaternion(rotation, 0.5f);

            if (_isSprinting)
            {
                _currentSpeed = _unit.Data.SprintSpeed;
            }
        }

        _unit.Animator.SetBool(_stringBus.ANIM_AIM_RIFLE, isAiming);
    }

    private Quaternion GetCameraLookRotation()
    {
        Vector3 cameraForward = _camera.transform.forward;
        return Quaternion.LookRotation(cameraForward);
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter user)
    {
        _inputMode = inputMode;
        _unit = user;
    }
}
