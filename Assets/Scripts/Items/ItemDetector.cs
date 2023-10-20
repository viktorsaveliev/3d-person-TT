using System;
using TMPro;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public event Action<Item> OnItemDetected;
    public event Action<Item> OnItemLost;

    [SerializeField] private GameObject _detectUI;
    [SerializeField] private TMP_Text _itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            UpdateItemData(item);
            ShowUI();

            OnItemDetected?.Invoke(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            HideUI();
            OnItemLost?.Invoke(item);
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
