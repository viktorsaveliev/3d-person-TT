using UnityEngine;
using Zenject;

public class UserCharacter : Unit
{
    [SerializeField] private Weapon _weapon;
    
    private IInputMode _inputMode;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        WeaponSystem weaponSystem = new();
        weaponSystem.EquipWeapon(_weapon);
        weaponSystem.OnWeaponReloadStateChanged += OnWeaponReloadStateChanged;
        AddSystem(weaponSystem);

        InputAttack inputAttack = new(_inputMode, weaponSystem);
        inputAttack.Init();
    }

    private void OnWeaponReloadStateChanged(bool isReloading)
    {
        StringBus stringBus = new();
        Animator.SetBool(stringBus.ANIM_RELOAD_RIFLE, isReloading);
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
