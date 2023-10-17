using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected UnitDataConfig _config;

    private readonly List<IUnitSystem> _systems = new();

    private HealthSystem _health;

    public UnitDataConfig Data => _config;

    public virtual void Init()
    {
        _health = new HealthSystem(_config.Health);
        _health.OnDead += OnDead;

        AddSystem(_health);
    }
    
    public void AddSystem(IUnitSystem system)
    {
        _systems.Add(system);
    }

    public void RemoveSystem(IUnitSystem system)
    {
        _systems.Remove(system);
    }

    public T GetSystem<T>() where T : IUnitSystem
    {
        return _systems.OfType<T>().FirstOrDefault();
    }

    private void OnDead()
    {
        gameObject.SetActive(false);
    }
}