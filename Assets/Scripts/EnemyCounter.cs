using System;
using TMPro;
using UnityEngine;
using Zenject;

public class EnemyCounter : MonoBehaviour
{
    public event Action OnAllEnemyKilled;

    [SerializeField] private TMP_Text _textCounter;

    private UnitFactory _unitFactory;
    private int _counter;

    public void Init()
    {
        foreach (var enemy in _unitFactory.Units)
        {
            enemy.GetSystem<HealthSystem>().OnDead += UpdateCounter;
        }
    }

    private void UpdateCounter()
    {
        if (++_counter >= UnitSpawner.MAX_UNITS)
        {
            OnAllEnemyKilled?.Invoke();
        }

        _textCounter.text = $"{_counter} / {UnitSpawner.MAX_UNITS}";
    }

    [Inject]
    public void Construct(UnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }
}
