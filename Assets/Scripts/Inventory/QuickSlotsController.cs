using System;
using Zenject;

public class QuickSlotsController
{
    public event Action OnWeaponChanged;

    private IInputMode _inputMode;
    private UserCharacter _user;

    private InventoryController _inventoryController;

    public void Init()
    {
        _inputMode.OnSelectWeapon += OnSelectSlot;
    }

    private void OnSelectSlot(int index)
    {
        Item item = _inventoryController.Inventory.Slots[index - 1].CurrentItem;

        if (item != null && item is Weapon weapon)
        {
            WeaponSystem weaponSystem = _user.GetSystem<WeaponSystem>();
            weaponSystem?.EquipWeapon(weapon);
            OnWeaponChanged?.Invoke();
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, UserCharacter user, InventoryController inventoryController)
    {
        _inputMode = inputMode;
        _user = user;
        _inventoryController = inventoryController;
    }
}
