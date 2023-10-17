
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
    }

    private void OnShot()
    {
        if (_weaponSystem == null) return;
        _weaponSystem.Shot();
    }
}
