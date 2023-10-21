using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;

    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;

    private Inventory _inventory;

    public void Init(Inventory inventory)
    {
        _inventory = inventory;

        _inventory.OnSelectSlot += UpdateInfo;
        _inventory.OnUnselectSlot += ResetInfo;
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _inventoryUI.SetActive(true);
    }

    public void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _inventory.UnselectSlot();
        _inventoryUI.SetActive(false);
    }

    private void UpdateInfo(InventorySlot slot)
    {
        Item item = slot.CurrentItem;
        Color color = _icon.color;

        _icon.color = new Color(color.r, color.g, color.b, 1);
        _icon.sprite = item.ItemData.Icon;
        _name.text = item.ItemData.Name;
        _description.text = item.ItemData.Description;
    }

    private void ResetInfo(InventorySlot slot)
    {
        Color color = _icon.color;
        _icon.color = new Color(color.r, color.g, color.b, 0);

        _icon.sprite = null;
        _name.text = string.Empty;
        _description.text = string.Empty;
    }
}
