using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public abstract class Unit : MonoBehaviour
{
    public Action OnInit;

    [SerializeField] private Animator _animator;
    [SerializeField] private UnitDataConfig _config;

    private readonly List<IUnitSystem> _systems = new();

    private AnimationCache _animCache;

    public UnitDataConfig Data { get => _config; }
    public Animator Animator { get => _animator; }
    public AnimationCache AnimCache => _animCache;

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
        _animator.SetTrigger(_animCache.DeathIndex);
    }

    [Inject]
    public void Construct(AnimationCache animCache)
    {
        _animCache = animCache;
    }
}