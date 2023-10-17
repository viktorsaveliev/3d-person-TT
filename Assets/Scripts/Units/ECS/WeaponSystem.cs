using UnityEngine;

public class WeaponSystem : IUnitSystem
{
    private Weapon _currentWeapon;

    public void EquipWeapon(Weapon weapon)
    {
        if (_currentWeapon != null)
        {

        }
        _currentWeapon = weapon;
        _currentWeapon.Init();
    }

    public void Shot()
    {
        if (_currentWeapon == null) return;

        _currentWeapon.TryShot();
    }
}
