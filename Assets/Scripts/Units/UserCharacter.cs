using UnityEngine;
using Zenject;

public class UserCharacter : Unit
{
    [SerializeField] private Transform _weaponContainer;

    private IInputMode _inputMode;

    private bool _isReloading;
    public bool IsReloading => _isReloading;

    public override void Init()
    {
        base.Init();

        WeaponSystem weaponSystem = new(_weaponContainer);
        weaponSystem.OnEquipWeapon += OnEquipWeapon;
        weaponSystem.OnUnequipWeapon += OnHideWeapon;
        weaponSystem.OnWeaponReloadStateChanged += OnWeaponReloadStateChanged;

        AddSystem(weaponSystem);

        InputAttack inputAttack = new(_inputMode, weaponSystem);
        inputAttack.Init();
    }

    private void OnWeaponReloadStateChanged(bool isReloading)
    {
        _isReloading = isReloading;
        Animator.SetBool(AnimCache.ReloadRifleIndex, isReloading);
    }

    private void OnEquipWeapon()
    {
        Animator.SetBool(AnimCache.WithRifleIndex, true);
    }

    private void OnHideWeapon()
    {
        Animator.SetBool(AnimCache.WithRifleIndex, false);
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
