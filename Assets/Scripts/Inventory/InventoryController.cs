using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(InventoryView))]
public class InventoryController : MonoBehaviour, IAmmoCounter
{
    [SerializeField] private InventoryDataConfig _config;

    [SerializeField] private Transform _slotsContainer;
    [SerializeField] private Transform _quickAccessSlotsContainer;

    [SerializeField] private InventorySlot _slotPrefab;
    [SerializeField] private Button _dropButton;

    private Inventory _inventory;
    private InventoryView _view;

    private IInputMode _inputMode;
    private ItemInteraction _itemInteraction;

    public Inventory Inventory => _inventory;

    public void Init()
    {
        _inventory = new(_config.SlotsCapacity);
        _view = GetComponent<InventoryView>();
        _view.Init(_inventory);

        CreateSlots();
    }

    private void OnEnable()
    {
        _inventory.OnSelectSlot += OnSelectSlot;
        _inventory.OnUnselectSlot += OnUnselectSlot;

        _inputMode.OnOpenInventory += OpenInventory;
        _itemInteraction.OnTryPickupItem += PickupItem;

        _dropButton.onClick.AddListener(DropItem);
    }

    private void OnDisable()
    {
        _inventory.OnSelectSlot -= OnSelectSlot;
        _inventory.OnUnselectSlot -= OnUnselectSlot;

        _inputMode.OnOpenInventory -= OpenInventory;
        _itemInteraction.OnTryPickupItem -= PickupItem;

        _dropButton.onClick.RemoveListener(DropItem);
    }

    public int GetAmmoCount(AmmoData.AmmoType ammoType)
    {
        int ammoCount = 0;
        foreach (var slot in _inventory.Slots)
        {
            if (slot.CurrentItem is Ammo ammo && ammo.AmmoType == ammoType)
            {
                ammoCount += ammo.CurrentAmount;
            }
        }
        return ammoCount;
    }

    public void SpendAmmo(AmmoData.AmmoType ammoType, int ammoCount)
    {
        int remainingAmmoCount = ammoCount;

        while (remainingAmmoCount > 0)
        {
            InventorySlot ammoSlot = GetAmmoSlotByType(ammoType);

            if (ammoSlot == null)
            {
                break;
            }

            int spentAmmo = ammoSlot.CurrentItem.SpendAmountAndGetSpent(remainingAmmoCount);
            remainingAmmoCount -= spentAmmo;

            if (remainingAmmoCount > 0)
            {
                ResetSlot(ammoSlot);
            }
            else
            {
                if (ammoSlot.CurrentItem.CurrentAmount <= 0)
                {
                    ResetSlot(ammoSlot);
                }

                break;
            }
        }

        if (remainingAmmoCount > 0)
        {
            throw new System.Exception("not enough ammo in inventory");
        }
    }

    private void DropItem() => _inventory.DropItem(_inventory.SelectedSlot);

    private void ResetSlot(InventorySlot slot)
    {
        slot.CurrentItem.Delete();
        slot.SetItem(null);
    }

    private InventorySlot GetAmmoSlotByType(AmmoData.AmmoType ammoType)
    {
        InventorySlot ammoSlot = null;

        foreach (var slot in _inventory.Slots)
        {
            if (slot.CurrentItem is Ammo ammo && ammo.AmmoType == ammoType)
            {
                ammoSlot = slot;
                break;
            }
        }

        return ammoSlot;
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _config.SlotsCapacity; i++)
        {
            InventorySlot slot;

            if (i < _config.QuickSlotsCapacity)
            {
                slot = Instantiate(_slotPrefab, _quickAccessSlotsContainer);
            }
            else
            {
                slot = Instantiate(_slotPrefab, _slotsContainer);
            }

            _inventory.AssignSlot(i, slot);
        }
    }

    private void OnSelectSlot(InventorySlot slot)
    {
        slot.ChangeColor(_config.SelectedSlotColor);
    }

    private void OnUnselectSlot(InventorySlot slot)
    {
        slot.ChangeColor(_config.RegularSlotColor);
    }

    private void OpenInventory()
    {
        _view.Show();
    }

    private void PickupItem(Item item)
    {
        if (_inventory.TryPickupItem(item))
        {
            item.HideAndExitTriggers();
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, ItemInteraction itemInteraction)
    {
        _inputMode = inputMode;
        _itemInteraction = itemInteraction;
    }
}
