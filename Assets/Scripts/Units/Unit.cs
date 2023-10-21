using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitDataConfig _config;

    private readonly List<IUnitSystem> _systems = new();

    public UnitDataConfig Data => _config;
    public Animator Animator => _animator;

    public virtual void Init()
    {
        HealthSystem health = new(_config.Health);
        health.OnDead += OnDead;

        AddSystem(health);
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

    public virtual void OnSpawn()
    {
        AISystem ai = GetSystem<AISystem>();
        ai?.Walk();
    }

    protected virtual void OnDead()
    {
        StringBus stringBus = new();
        _animator.SetTrigger(stringBus.ANIM_DEATH_2);
    }
}