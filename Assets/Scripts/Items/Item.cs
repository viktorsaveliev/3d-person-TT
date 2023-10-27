using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemDataConfig _itemConfig;

    private int _currentAmount;
    private Rigidbody _rigidbody;
    private Collider _collider;

    public int CurrentAmount => _currentAmount;
    public ItemDataConfig ItemData => _itemConfig;

    public virtual void Init()
    {
        _currentAmount = _itemConfig.Amount;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public int GiveAmountAndGetResidue(int amount)
    {
        if (amount < 1)
        {
            throw new Exception("Mismatch amount value");
        }

        int residueAmount;
        _currentAmount += amount;

        if (_currentAmount > _itemConfig.MaxAmount)
        {
            residueAmount = _currentAmount - _itemConfig.MaxAmount;
            _currentAmount = _itemConfig.MaxAmount;
        }
        else
        {
            residueAmount = 0;
        }

        return residueAmount;
    }

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

    public void SetAmount(int amount)
    {
        if (amount < 1 || amount > _itemConfig.MaxAmount)
        {
            throw new Exception("Mismatch amount value");
        }

        _currentAmount = amount;
    }

    public void Show(bool onHands)
    {
        if (onHands)
        {
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }
        else
        {
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;
            
            _collider.enabled = true;
        }

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
