using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitFactory : MonoBehaviour
{
    public event Action<Unit> OnUnitCreated;

    [SerializeField] private Unit[] _unitPrefab;
    [SerializeField] private Transform _container;

    private readonly List<Unit> _units = new();
    public IReadOnlyList<Unit> Units => _units;

    [Inject] private readonly DiContainer _diContainer;

    public enum UnitType
    {
        Regular
    }

    public Unit CreateUnit(UnitType unitType, Vector3 position)
    {
        GameObject unitObject = _diContainer.InstantiatePrefab(_unitPrefab[(int)unitType], _container);
        Unit unit = unitObject.GetComponent<Unit>();
        unit.transform.position = position;
        unit.Init();

        _units.Add(unit);
        OnUnitCreated?.Invoke(unit);
        return unit;
    }
}
