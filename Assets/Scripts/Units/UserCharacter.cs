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
        AddSystem(weaponSystem);
        weaponSystem.EquipWeapon(_weapon);

        InputAttack inputAttack = new(_inputMode, weaponSystem);
        inputAttack.Init();
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
