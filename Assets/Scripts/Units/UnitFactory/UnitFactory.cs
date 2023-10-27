using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitFactory : MonoBehaviour
{
    public event Action<Unit> OnUnitCreated;

    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _container;

    private readonly List<Unit> _units = new();
    public IReadOnlyList<Unit> Units => _units;

    public enum UnitType
    {
        Zombie
    }

    [Inject] private readonly DiContainer _diContainer;

    public Unit CreateUnit(UnitType unitType)
    {
        Unit unit = _diContainer.InstantiatePrefab(_unit, _container).GetComponent<Unit>();
        unit.Init();
        _units.Add(unit);

        unit.gameObject.SetActive(false);
        OnUnitCreated?.Invoke(unit);
        return unit;
    }
}
