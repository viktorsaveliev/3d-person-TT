using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event Action<Item> OnPickupItem;
    public event Action<Item> OnDropItem;

    public event Action<InventorySlot> OnSelectSlot;
    public event Action<InventorySlot> OnUnselectSlot;

    private readonly int _slotsCapacity;
    private readonly InventorySlot[] _slots;

    private InventorySlot _selectedSlot;

    public IReadOnlyList<InventorySlot> Slots => _slots;

    public Inventory(int slotsCapacity)
    {
        _slotsCapacity = slotsCapacity;
        _slots = new InventorySlot[_slotsCapacity];
    }

    public void AssignSlot(int index, InventorySlot slot)
    {
        _slots[index] = slot;
        slot.OnClick += OnClickSlot;
    }

    public bool TryPickupItem(Item item)
    {
        InventorySlot freeSlot = GetFreeSlot();

        if (freeSlot == null)
        {
            new Exception("You don't have free slots");
            return false;
        }

        freeSlot.SetItem(item);
        OnPickupItem?.Invoke(item);
        return true;
    }

    public void DropItem(int slot)
    {
        OnDropItem?.Invoke(_slots[slot].CurrentItem);
        _slots[slot].SetItem(null);
    }

    public void UseItem(int slot)
    {
        _slots[slot].CurrentItem.Use();
    }

    private void OnClickSlot(InventorySlot slot)
    {
        if (_selectedSlot != null)
        {
            if (_selectedSlot == slot)
            {
                UnselectSlot(slot);
            }
            else
            {
                if (slot.CurrentItem != null)
                {
                    UnselectSlot(_selectedSlot);
                    SelectSlot(slot);
                }
                else
                {
                    slot.SetItem(_selectedSlot.CurrentItem);
                    _selectedSlot.DeleteItem();

                    UnselectSlot(_selectedSlot);
                }
            }
        }
        else
        {
            if (slot.CurrentItem != null)
            {
                SelectSlot(slot);
            } 
        }
    }

    private void SelectSlot(InventorySlot slot)
    {
        _selectedSlot = slot;
        OnSelectSlot?.Invoke(slot);
    }

    private void UnselectSlot(InventorySlot slot)
    {
        _selectedSlot = null;
        OnUnselectSlot?.Invoke(slot);
    }

    private InventorySlot GetFreeSlot()
    {
        InventorySlot freeSlot = null;

        foreach (var slot in _slots)
        {
            if (slot.CurrentItem != null) continue;
            freeSlot = slot;
            break;
        }

        return freeSlot;
    }
}