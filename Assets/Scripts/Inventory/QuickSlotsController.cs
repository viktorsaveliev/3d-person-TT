using UnityEngine;
using Zenject;

public class QuickSlotsController : MonoBehaviour
{
    private IInputMode _inputMode;
    private UserCharacter _user;

    private InventoryController _inventoryController;

    private void OnEnable()
    {
        _inputMode.OnSelectWeapon += OnSelectSlot;
    }

    private void OnDisable()
    {
        _inputMode.OnSelectWeapon -= OnSelectSlot;
    }

    private void OnSelectSlot(int index)
    {
        Item item = _inventoryController.Slots[index - 1].CurrentItem;

        if (item != null && item is Weapon weapon)
        {
            WeaponSystem weaponSystem = _user.GetSystem<WeaponSystem>();
            weaponSystem?.EquipWeapon(weapon);
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
