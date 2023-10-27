using UnityEngine;
using Zenject;

[RequireComponent(typeof(Unit))]
public class InputMovement : MonoBehaviour
{
    [SerializeField] private Transform _unitTransform;

    private UserCharacter _unit;
    private IInputMode _inputMode;
    private AnimationCache _animCache;

    private Camera _camera;
    private Rigidbody _rigidbody;

    private readonly int _groundLayer = 3;

    private float _currentSpeed;

    private bool _isAiming;
    private bool _isGrounded;
    private bool _isSprinting;

    private int _groundCollisionCount = 0;

    private bool IsMoveActive => Cursor.lockState == CursorLockMode.Locked;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        _inputMode.OnJump += StartJump;
    }

    private void OnDisable()
    {
        _inputMode.OnRotated -= Rotate;
        _inputMode.OnMove -= Move;
        _inputMode.OnAimed -= Aim;
        _inputMode.OnSprint -= Sprint;
        _inputMode.OnJump -= StartJump;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            if (_groundCollisionCount <= 0)
            {
                if (_isSprinting)
                {
                    _unit.Animator.SetBool(_animCache.SprintIndex, true);
                }

                _isGrounded = true;
                _unit.Animator.speed = 1f;
            }
            _groundCollisionCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            if (--_groundCollisionCount <= 0)
            {
                _isGrounded = false;
                _groundCollisionCount = 0;
            }
        }
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
            _unit.Animator.SetFloat(_animCache.MoveIndex, _rigidbody.velocity.magnitude);
        }
        else
        {
            _unit.Animator.SetFloat(_animCache.MoveIndex, 0);
        }
    }

    private void StartJump()
    {
        if (!_isGrounded) return;
        _rigidbody.velocity *= 0.5f;
        _unit.Animator.SetBool(_animCache.SprintIndex, false);
        _unit.Animator.SetTrigger(_animCache.JumpIndex);
    }

    private void Jump() // For animation event
    {
        if (!_isGrounded) return;

        float jumpForce = 100f;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _isGrounded = false;
    }

    private void PauseAnimation() // For animation event
    {
        _unit.Animator.speed = 0;
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
        _unit.Animator.SetBool(_animCache.SprintIndex, isSprinting);
    }

    private void Aim(bool isAiming)
    {
        if (!IsMoveActive || !_isGrounded) return;

        _isAiming = isAiming;
        _currentSpeed = isAiming ? _unit.Data.AimSpeed : _unit.Data.RegularSpeed;

        if (!isAiming)
        {
            Quaternion rotation = Quaternion.Euler(0, _unitTransform.rotation.eulerAngles.y, _unitTransform.rotation.eulerAngles.z);
            _unitTransform.rotation = rotation;

            if (_isSprinting)
            {
                _currentSpeed = _unit.Data.SprintSpeed;
            }
        }

        _unit.Animator.SetBool(_animCache.AimRifleIndex, isAiming);
    }

    private Quaternion GetCameraLookRotation()
    {
        Vector3 cameraForward = _camera.transform.forward;
        return Quaternion.LookRotation(cameraForward);
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter user, AnimationCache animCache)
    {
        _inputMode = inputMode;
        _unit = user;
        _animCache = animCache;
    }
}
