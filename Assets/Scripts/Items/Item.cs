using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemDataConfig _itemConfig;

    private int _currentAmount;

    public int CurrentAmount => _currentAmount;
    public ItemDataConfig ItemData => _itemConfig;

    public virtual void Init()
    {
        _currentAmount = _itemConfig.Amount;
    }

    public abstract void Use();

    public int SpendAmountAndGetSpent(int amount)
    {
        if (amount < 1)
        {
            throw new Exception("Mismatch amount value");
        }

        if (amount > _currentAmount)
        {
            int spent = _currentAmount;
            _currentAmount = 0;
            return spent;
        }

        _currentAmount -= amount;
        return amount;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void HideAndExitTriggers()
    {
        // костыль, чтоб вызвался OnTriggerExit
        transform.position = Vector3.up * 20;
        Invoke(nameof(Hide), 0.2f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
