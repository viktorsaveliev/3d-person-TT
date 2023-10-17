using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AimUI : MonoBehaviour
{
    [SerializeField] private GameObject _aimObject;

    [SerializeField] private TMP_Text _ammo;
    [SerializeField] private TMP_Text _gunName;

    [SerializeField] private Image _healthProgressBar;

    private IInputMode _inputMode;

    private void OnEnable()
    {
        _inputMode.OnAimed += OnAimStateChange;
    }

    private void OnDisable()
    {
        _inputMode.OnAimed -= OnAimStateChange;
    }

    private void OnAimStateChange(bool isAiming)
    {
        if (isAiming)
        {
            _aimObject.SetActive(true);
        }
        else
        {
            _aimObject.SetActive(false);
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
