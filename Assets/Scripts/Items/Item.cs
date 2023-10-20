using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemDataConfig _itemConfig;

    public ItemDataConfig ItemData => _itemConfig;

    public abstract void Use();

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void HideWithDelay()
    {
        // костыль, чтоб вызвался OnTriggerExit
        transform.position = Vector3.up * 20;
        Invoke(nameof(Hide), 0.2f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
