using System;
using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public event Action<Unit> OnUnitSpawned;
    public const int MAX_UNITS = 15;

    [SerializeField] private Collider _spawnArea;
    [SerializeField] private UnitFactory _factory;

    private readonly WaitForSeconds _delayForSpawn = new(4f);

    private bool _isSpawnerActive;

    public void Init()
    {
        _isSpawnerActive = true;
        CreateUnits();
        StartCoroutine(SpawnTimer());
    }

    private void CreateUnits()
    {
        for (int i = 0; i < MAX_UNITS; i++)
        {
            _factory.CreateUnit(UnitFactory.UnitType.Zombie);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if (_spawnArea == null)
        {
            Debug.LogError("Collider is null.");
            return Vector3.zero;
        }

        Bounds bounds = _spawnArea.bounds;
        Vector3 randomPoint = new(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 1f,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPoint;
    }

    private IEnumerator SpawnTimer()
    {
        while (_isSpawnerActive)
        {
            yield return _delayForSpawn;
            Unit unit = GetFreeUnit();
            if (unit != null)
            {
                SpawnUnit(unit, GetRandomSpawnPoint());
            }
        }
    }

    private void SpawnUnit(Unit unit, Vector3 position)
    {
        unit.transform.position = position;
        unit.gameObject.SetActive(true);
        unit.OnSpawn();

        OnUnitSpawned?.Invoke(unit);
    }

    private Unit GetFreeUnit()
    {
        Unit freeUnit = null;

        foreach (var unit in _factory.Units)
        {
            if (unit.gameObject.activeSelf) continue;
            freeUnit = unit;
            break;
        }

        return freeUnit;
    }
}
