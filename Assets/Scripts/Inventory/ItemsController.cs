using UnityEngine;
using Zenject;

public class ItemsController
{
    private InventoryController _controller;
    private UserCharacter _user;

    private readonly float _offsetMultiplayer = 1.2f; 

    public void Init()
    {
        _controller.Inventory.OnDropItem += SpawnItem;
    }

    private void SpawnItem(Item item)
    {
        Vector3 offset = (_user.transform.forward * _offsetMultiplayer) + (_user.transform.up * _offsetMultiplayer);

        item.transform.parent = null;
        item.transform.position = _user.transform.position + offset;
        item.Show(false);
    }

    [Inject]
    public void Construct(InventoryController controller, UserCharacter user)
    {
        _controller = controller;
        _user = user;
    }
}
