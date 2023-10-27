using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public event Action<Item> OnItemDetected;
    public event Action<Item> OnItemLost;

    [SerializeField] private GameObject _detectUI;
    [SerializeField] private TMP_Text _itemName;

    private readonly HashSet<Item> _nearestItems = new();
    public IReadOnlyCollection<Item> NearestItems => _nearestItems;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (_nearestItems.Add(item))
            {
                UpdateItemData(item);
                ShowUI();

                OnItemDetected?.Invoke(item);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (_nearestItems.Remove(item))
            {
                Item anotherItem = _nearestItems.FirstOrDefault();
                if (anotherItem != null)
                {
                    UpdateItemData(anotherItem);
                }
                else
                {
                    HideUI();
                }

                OnItemLost?.Invoke(item);
            }
        }
    }

    private void ShowUI()
    {
        _detectUI.SetActive(true);
    }

    private void HideUI()
    {
        _detectUI.SetActive(false);
    }

    private void UpdateItemData(Item item)
    {
        _itemName.text = $"{item.ItemData.name}";
    }
}
