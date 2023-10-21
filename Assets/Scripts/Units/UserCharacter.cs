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
        weaponSystem.OnHideWeapon += OnHideWeapon;
        weaponSystem.OnWeaponReloadStateChanged += OnWeaponReloadStateChanged;

        AddSystem(weaponSystem);

        InputAttack inputAttack = new(_inputMode, weaponSystem);
        inputAttack.Init();
    }

    private void OnWeaponReloadStateChanged(bool isReloading)
    {
        _isReloading = isReloading;

        StringBus stringBus = new();
        Animator.SetBool(stringBus.ANIM_RELOAD_RIFLE, isReloading);
    }

    private void OnEquipWeapon()
    {
        StringBus stringBus = new();
        Animator.SetBool(stringBus.ANIM_WITH_RIFLE, true);
    }

    private void OnHideWeapon()
    {
        StringBus stringBus = new();
        Animator.SetBool(stringBus.ANIM_WITH_RIFLE, false);
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
