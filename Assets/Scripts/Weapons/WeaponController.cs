using Zenject;

public class WeaponController
{
    private InventoryController _inventoryController;
    private UserCharacter _user;

    public void Init()
    {
        _inventoryController.Inventory.OnDropItem += HideWeapon;
    }

    private void HideWeapon(Item item)
    {
        if (item is Weapon weapon)
        {
            WeaponSystem weaponSystem = _user.GetSystem<WeaponSystem>();
            if (weapon == weaponSystem.CurrentWeapon)
            {
                weaponSystem.UnequipWeapon(false);
            }
        }
    }

    [Inject]
    public void Construct(InventoryController controller, UserCharacter user)
    {
        _inventoryController = controller;
        _user = user;
    }
}
