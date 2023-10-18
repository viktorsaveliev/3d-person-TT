using System;

public class WeaponSystem : IUnitSystem
{
    public event Action<bool> OnWeaponReloadStateChanged;

    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    public void EquipWeapon(Weapon weapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnReloadStateChanged -= OnRealod;
        }
        _currentWeapon = weapon;
        _currentWeapon.Init();
        _currentWeapon.OnReloadStateChanged += OnRealod;
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
