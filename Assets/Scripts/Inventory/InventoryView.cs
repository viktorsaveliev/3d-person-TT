using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUI;
    
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

        _inventoryUI.SetActive(false);
    }
}
