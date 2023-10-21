
public class InputAttack
{
    private readonly IInputMode _inputMode;
    private readonly WeaponSystem _weaponSystem;

    public InputAttack(IInputMode inputMode, WeaponSystem weaponSystem)
    {
        _inputMode = inputMode;
        _weaponSystem = weaponSystem;
    }

    public void Init()
    {
        _inputMode.OnShot += OnShot;
        _inputMode.OnReloadWeapon += OnReload;
    }

    private void OnShot()
    {
        if (_weaponSystem == null || _weaponSystem.CurrentWeapon == null) return;
        _weaponSystem.Shot();
    }

    private void OnReload()
    {
        _weaponSystem.CurrentWeapon.StartReloading();
    }
}
