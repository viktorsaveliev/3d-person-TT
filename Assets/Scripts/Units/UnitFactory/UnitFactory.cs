using System;
using System.Collections.Generic;
using UnityEngine;

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

    public Unit CreateUnit(UnitType unitType)
    {
        Unit unit = Instantiate(_unit, _container);
        unit.Init();
        _units.Add(unit);

        unit.gameObject.SetActive(false);
        OnUnitCreated?.Invoke(unit);
        return unit;
    }
}
