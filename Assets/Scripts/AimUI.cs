using DG.Tweening;
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

    private UserCharacter _userUnit;
    private Weapon _currentWeapon;

    private IInputMode _inputMode;

    private void OnEnable()
    {
        _inputMode.OnAimed += OnAimStateChange;
        _inputMode.OnShot += OnPlayerShot;
    }

    private void OnDisable()
    {
        _inputMode.OnAimed -= OnAimStateChange;
        _inputMode.OnShot -= OnPlayerShot;
    }

    private void OnPlayerShot()
    {
        UpdateAmmo();
    }

    private void OnAimStateChange(bool isAiming)
    {
        if (isAiming)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _currentWeapon = _userUnit.GetSystem<WeaponSystem>()?.CurrentWeapon;
        _currentWeapon.OnReloadStateChanged += OnReload;

        _gunName.text = $"{_currentWeapon.Data.Name}";
        _aimObject.SetActive(true);
        
        UpdateAmmo();
    }

    private void Hide()
    {
        _currentWeapon.OnReloadStateChanged -= OnReload;
        _aimObject.SetActive(false);
    }

    private void UpdateAmmo()
    {
        if (_currentWeapon != null)
        {
            _ammo.text = $"{_currentWeapon.CurrentAmmo} / {_currentWeapon.Data.MaxAmmo}";

            float percentage = (float)(_currentWeapon.CurrentAmmo - 1) / (_currentWeapon.Data.MaxAmmo - 1);
            _healthProgressBar.fillAmount = percentage;
        }
    }

    private void OnReload(bool isReloading)
    {
        if (!isReloading)
        {
            UpdateAmmo();
        }
        else
        {
            _healthProgressBar.DOFillAmount(1f, _currentWeapon.Data.ReloadTime);
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter userUnit)
    {
        _inputMode = inputMode;
        _userUnit = userUnit;
    }
}
