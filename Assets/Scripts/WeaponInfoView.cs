using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeaponInfoView : MonoBehaviour
{
    [SerializeField] private GameObject _weaponInfo;

    [SerializeField] private TMP_Text _ammo;
    [SerializeField] private TMP_Text _gunName;

    [SerializeField] private Image _ammoProgressBar;

    private UserCharacter _userUnit;
    private Weapon _currentWeapon;
    private QuickSlotsController _quickSlots;
    private InventoryController _inventoryController;

    private IInputMode _inputMode;
    private IAmmoCounter _ammoCounter;

    private int _currentAllAmmoCount;

    public void Init()
    {
        _inputMode.OnShot += OnPlayerShot;

        _quickSlots.OnWeaponChanged += UpdateWeaponInfo;
        _inventoryController.Inventory.OnPickupItem += UpdateAllAmmoCount;
        _inventoryController.Inventory.OnDropItem += UpdateAllAmmoCount;

        WeaponSystem weaponSystem = _userUnit.GetSystem<WeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.OnUnequipWeapon += Hide;
        }
    }

    private void UpdateAllAmmoCount(Item item)
    {
        if (_currentWeapon == null) return;

        if (item is Ammo ammo && _currentWeapon.AmmoType == ammo.AmmoType)
        {
            _currentAllAmmoCount = _ammoCounter.GetAmmoCount(_currentWeapon.AmmoType);
            UpdateCurrentAmmoCount();
        }
    }

    private void UpdateCurrentAmmoCount()
    {
        if (_currentWeapon != null)
        {
            _ammo.text = $"{_currentWeapon.CurrentAmmoCapacity} / {_currentAllAmmoCount}";

            float percentage = (float)_currentWeapon.CurrentAmmoCapacity / _currentWeapon.WeaponData.MaxAmmo;
            _ammoProgressBar.fillAmount = percentage;
        }
    }

    private void UpdateWeaponInfo()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnReloadStateChanged -= OnReload;
        }

        _currentWeapon = _userUnit.GetSystem<WeaponSystem>()?.CurrentWeapon;

        if (_currentWeapon != null)
        {
            Show();

            _currentWeapon.OnReloadStateChanged += OnReload;
            _currentAllAmmoCount = _ammoCounter.GetAmmoCount(_currentWeapon.AmmoType);
            _gunName.text = $"{_currentWeapon.ItemData.Name}";
        }
        else
        {
            Hide();
        }

        UpdateCurrentAmmoCount();
    }

    private void OnPlayerShot()
    {
        UpdateCurrentAmmoCount();
    }

    private void Show()
    {
        if (_currentWeapon != null)
        {
            _weaponInfo.SetActive(true);
            UpdateCurrentAmmoCount();
        }
    }

    private void Hide()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnReloadStateChanged -= OnReload;

            _weaponInfo.SetActive(false);
            
        }
    }

    private void OnReload(bool isReloading)
    {
        if (!isReloading)
        {
            _currentAllAmmoCount = _ammoCounter.GetAmmoCount(_currentWeapon.AmmoType);
            UpdateCurrentAmmoCount();
        }
        else
        {
            _ammoProgressBar.DOFillAmount(1f, _currentWeapon.WeaponData.ReloadTime);
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, 
        UserCharacter userUnit, 
        QuickSlotsController quickSlots, 
        IAmmoCounter ammoCounter, 
        InventoryController inventoryController)
    {
        _inputMode = inputMode;
        _userUnit = userUnit;
        _quickSlots = quickSlots;
        _ammoCounter = ammoCounter;
        _inventoryController = inventoryController;
    }
}
