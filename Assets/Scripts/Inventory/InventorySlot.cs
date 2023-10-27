using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    public event Action<InventorySlot> OnClick;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private TMP_Text _amountText;

    private Button _slotButton;
    private Item _item;

    public Item CurrentItem => _item;

    private void Awake()
    {
        _slotButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _slotButton.onClick.AddListener(OnClickSlot);
    }

    private void OnDisable()
    {
        _slotButton.onClick.RemoveListener(OnClickSlot);
    }

    public void SetItem(Item item)
    {
        _item = item;
        UpdateData();
    }

    public void DeleteItem()
    {
        _item = null;
        UpdateData();
    }

    public void ChangeColor(Color color)
    {
        _slotButton.image.color = color;
    }

    public void UpdateData()
    {
        if (_item == null)
        {
            _icon.sprite = _defaultIcon;
            _amountText.text = string.Empty;
        }
        else
        {
            _amountText.text = $"{(_item.CurrentAmount > 1 ? _item.CurrentAmount : string.Empty)}";
            _icon.sprite = _item.ItemData.Icon;
        }
    }

    private void OnClickSlot()
    {
        OnClick?.Invoke(this);
    }
}
