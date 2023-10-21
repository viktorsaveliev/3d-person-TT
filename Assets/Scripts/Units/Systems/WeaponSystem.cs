using System;
using UnityEngine;

public class WeaponSystem : IUnitSystem
{
    public event Action OnEquipWeapon;
    public event Action OnHideWeapon;
    public event Action<bool> OnWeaponReloadStateChanged;

    private readonly Transform _weaponContainer;

    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    public WeaponSystem(Transform weaponContainer)
    {
        _weaponContainer = weaponContainer;
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (_currentWeapon == weapon) return;

        if (_currentWeapon != null)
        {
            _currentWeapon.Hide();
            _currentWeapon.OnReloadStateChanged -= OnRealod;
        }

        _currentWeapon = weapon;

        _currentWeapon.OnReloadStateChanged += OnRealod;
        _currentWeapon.transform.parent = _weaponContainer;
        _currentWeapon.transform.localPosition = _currentWeapon.transform.localEulerAngles = Vector3.zero;

        _currentWeapon.Show();

        OnEquipWeapon?.Invoke();
    }

    public void HideWeapon()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.Hide();
            _currentWeapon.OnReloadStateChanged -= OnRealod;
            _currentWeapon = null;
        }

        OnHideWeapon?.Invoke();
    }

    public void Shot()
    {
        if (_currentWeapon == null) return;

        _currentWeapon.TryShot();
    }

    private void OnRealod(bool isReloading)
    {
        OnWeaponReloadStateChanged?.Invoke(isReloading);
    }
}
