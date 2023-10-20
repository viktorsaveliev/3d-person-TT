using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    public event Action<InventorySlot> OnClick;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _defaultIcon;

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
        UpdateIcon();
    }

    public void DeleteItem()
    {
        _item = null;
        UpdateIcon();
    }

    public void ChangeColor(Color color)
    {
        _slotButton.image.color = color;
    }

    private void UpdateIcon()
    {
        if (_item == null)
        {
            _icon.sprite = _defaultIcon;
        }
        else
        {
            _icon.sprite = _item.ItemData.Icon;
        }
    }

    private void OnClickSlot()
    {
        OnClick?.Invoke(this);
    }
}
