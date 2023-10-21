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
    private QuickSlotsController _quickSlots;

    private IInputMode _inputMode;

    private bool _isAiming;

    private void OnEnable()
    {
        _inputMode.OnAimed += OnAimStateChange;
        _inputMode.OnShot += OnPlayerShot;

        _quickSlots.OnWeaponChanged += UpdateData;
    }

    private void OnDisable()
    {
        _inputMode.OnAimed -= OnAimStateChange;
        _inputMode.OnShot -= OnPlayerShot;

        _quickSlots.OnWeaponChanged -= UpdateData;
    }

    private void UpdateData()
    {
        if (!_isAiming) return;

        _currentWeapon = _userUnit.GetSystem<WeaponSystem>()?.CurrentWeapon;
        _gunName.text = $"{_currentWeapon.ItemData.Name}";
        UpdateAmmo();
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

        _isAiming = isAiming;
    }

    private void Show()
    {
        _currentWeapon = _userUnit.GetSystem<WeaponSystem>()?.CurrentWeapon;
        if (_currentWeapon != null)
        {
            _currentWeapon.OnReloadStateChanged += OnReload;

            _gunName.text = $"{_currentWeapon.ItemData.Name}";
            _aimObject.SetActive(true);

            UpdateAmmo();
        }
    }

    private void Hide()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnReloadStateChanged -= OnReload;
            _aimObject.SetActive(false);
        }
    }

    private void UpdateAmmo()
    {
        if (_currentWeapon != null)
        {
            _ammo.text = $"{_currentWeapon.CurrentAmmo} ammo";

            float percentage = (float)(_currentWeapon.CurrentAmmo - 1) / (_currentWeapon.WeaponData.MaxAmmo - 1);
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
            _healthProgressBar.DOFillAmount(1f, _currentWeapon.WeaponData.ReloadTime);
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter userUnit, QuickSlotsController quickSlots)
    {
        _inputMode = inputMode;
        _userUnit = userUnit;
        _quickSlots = quickSlots;
    }
}
