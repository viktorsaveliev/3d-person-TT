using System;
using UnityEngine;
using Zenject;

public class ItemInteraction : MonoBehaviour
{
    public event Action<Item> OnTryPickupItem;

    private IInputMode _inputMode;
    private ItemDetector _itemDetector;

    private Item _currentItem;

    private void OnEnable()
    {
        _itemDetector.OnItemDetected += OnItemDetected;
        _itemDetector.OnItemLost += OnItemLost;

        _inputMode.OnInteraction += Action;
    }

    private void OnDisable()
    {
        _itemDetector.OnItemDetected -= OnItemDetected;
        _itemDetector.OnItemLost -= OnItemLost;

        _inputMode.OnInteraction -= Action;
    }

    private void OnItemDetected(Item item)
    {
        _currentItem = item;
    }

    private void OnItemLost(Item item)
    {
        _currentItem = null;
    }

    private void Action()
    {
        if (_currentItem == null) return;

        OnTryPickupItem?.Invoke(_currentItem);
    }

    [Inject]
    public void Construct(IInputMode inputMode, ItemDetector itemDetector)
    {
        _inputMode = inputMode;
        _itemDetector = itemDetector;
    }
}
