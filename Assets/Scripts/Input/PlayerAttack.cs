using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    private IInputMode _inputMode;
    private bool _isAiming;

    private void OnEnable()
    {
        _inputMode.OnAimed += Aim;
    }

    private void OnDisable()
    {
        _inputMode.OnAimed -= Aim;
    }

    private void Aim(bool isAiming)
    {
        _isAiming = isAiming;
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
