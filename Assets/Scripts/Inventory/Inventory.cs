using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    public event Action<Item> OnPickupItem;
    public event Action<Item> OnDropItem;

    public event Action<InventorySlot> OnSelectSlot;
    public event Action<InventorySlot> OnUnselectSlot;

    private readonly InventorySlot[] _slots;

    private InventorySlot _selectedSlot;

    public InventorySlot SelectedSlot => _selectedSlot;
    public IReadOnlyList<InventorySlot> Slots => _slots;

    public Inventory(int slotsCapacity)
    {
        _slots = new InventorySlot[slotsCapacity];
    }

    public void AssignSlot(int index, InventorySlot slot)
    {
        _slots[index] = slot;
        slot.OnClick += OnClickSlot;
    }

    public bool TryPickupItem(Item item)
    {
        int residueAmount = TryMergeAndGetResidue(item);

        if (residueAmount > 0)
        {
            InventorySlot freeSlot = GetFreeSlot();

            if (freeSlot == null)
            {
                Debug.Log("You don't have free slots");
                return false;
            }

            item.SetAmount(residueAmount);
            freeSlot.SetItem(item);
        }

        OnPickupItem?.Invoke(item);
        return true;
    }

    public void DropItem(InventorySlot slot)
    {
        if (slot == null || slot.CurrentItem == null) return;

        Item currentItem = slot.CurrentItem;
        slot.SetItem(null);

        if (slot == _selectedSlot)
        {
            UnselectSlot();
        }

        OnDropItem?.Invoke(currentItem);
    }

    public void UnselectSlot()
    {
        if (_selectedSlot == null) return;
        OnUnselectSlot?.Invoke(_selectedSlot);
        _selectedSlot = null;
    }

    private int TryMergeAndGetResidue(Item item)
    {
        int itemAmount = item.CurrentAmount;

        if (item.ItemData.MaxAmount < 2)
        {
            return itemAmount;
        }

        while (itemAmount > 0)
        {
            InventorySlot existingSlot = FindSpaceAmountAvailableSlot(item);

            if (existingSlot != null)
            {
                int spaceAvailable = existingSlot.CurrentItem.ItemData.MaxAmount - existingSlot.CurrentItem.CurrentAmount;
                if (spaceAvailable > 0)
                {
                    itemAmount = existingSlot.CurrentItem.GiveAmountAndGetResidue(itemAmount);
                    existingSlot.UpdateData();
                }

                if (itemAmount <= 0)
                {
                    break;
                }
            }
            else
            {
                return itemAmount;
            }
        }

        return itemAmount;
    }

    private InventorySlot FindSpaceAmountAvailableSlot<T>(T itemType) where T : Item
    {
        InventorySlot foundSlot = _slots.FirstOrDefault(slot =>
        {
            if (slot.CurrentItem == null) return false;

            bool isMatchingType = slot.CurrentItem.GetType() == itemType.GetType();
            bool isAmountAvailable = slot.CurrentItem.CurrentAmount < slot.CurrentItem.ItemData.MaxAmount;

            if (!isMatchingType || !isAmountAvailable) return false;

            if (slot.CurrentItem is Ammo ammo1 && itemType is Ammo ammo2 &&
                ammo1.AmmoType != ammo2.AmmoType)
            {
                return false;
            }

            return true;
        });

        return foundSlot;
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