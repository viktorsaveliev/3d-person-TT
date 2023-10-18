using System;
using UnityEngine;

public class HealthSystem : IUnitSystem
{
    public event Action<int> OnTakedDamage; 
    public event Action OnDead; 

    private int _health;
    private int _maxHealth;
    private Vector3 _lastHitPosition;

    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public Vector3 LastHitPosition => _lastHitPosition;

    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    public void SetHealth(int health)
    {
        if (health < 1 || health > _maxHealth)
        {
            throw new ArgumentException();
        }

        _health = health;
    }

    public void SetMaxHealth(int health)
    {
        if (health < 1)
        {
            throw new ArgumentException();
        }

        _maxHealth = health;
    }

    public void TakeDamage(Vector3 hitPosition, int damage)
    {
        _lastHitPosition = hitPosition;
        _health -= damage;

        if (_health <= 0)
        {
            OnDead?.Invoke();
        }

        OnTakedDamage?.Invoke(damage);
    }
}
