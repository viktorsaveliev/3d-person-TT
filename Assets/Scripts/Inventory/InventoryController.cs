using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(InventoryView))]
public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryDataConfig _config;

    [SerializeField] private Transform _slotsContainer;
    [SerializeField] private Transform _quickAccessSlotsContainer;

    [SerializeField] private InventorySlot _slotPrefab;

    private Inventory _inventory;
    private InventoryView _view;

    private IInputMode _inputMode;
    private ItemInteraction _itemInteraction;

    public IReadOnlyList<InventorySlot> Slots => _inventory.Slots;

    private void Awake()
    {
        _inventory = new(_config.SlotsCapacity);
        _view = GetComponent<InventoryView>();
        CreateSlots();
    }

    private void OnEnable()
    {
        _inventory.OnSelectSlot += OnSelectSlot;
        _inventory.OnUnselectSlot += OnUnselectSlot;

        _inputMode.OnOpenInventory += OnOpenInventory;
        _itemInteraction.OnTryPickupItem += PickupItem;
    }

    private void OnDisable()
    {
        _inventory.OnSelectSlot -= OnSelectSlot;
        _inventory.OnUnselectSlot -= OnUnselectSlot;

        _inputMode.OnOpenInventory -= OnOpenInventory;
        _itemInteraction.OnTryPickupItem -= PickupItem;
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

    private void OnOpenInventory()
    {
        _view.Show();
    }

    private void PickupItem(Item item)
    {
        if (_inventory.TryPickupItem(item))
        {
            item.HideWithDelay();
        }
    }

    [Inject]
    public void Construct(IInputMode inputMode, ItemInteraction itemInteraction)
    {
        _inputMode = inputMode;
        _itemInteraction = itemInteraction;
    }
}
