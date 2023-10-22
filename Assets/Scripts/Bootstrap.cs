using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    private GameStatus _gameStats;
    private TakeDamageView _takeDamageView;
    private EnemyCounter _enemyCounter;
    private UserCharacter _userCharacter;
    private UnitSpawner _unitSpawner;
    private QuickSlotsController _quickSlots;


    [Inject]
    public void Construct(GameStatus gameStats, 
        EnemyCounter counter, 
        TakeDamageView takeDamageView, 
        UserCharacter userCharacter, 
        UnitSpawner unitSpawner,
        QuickSlotsController quickSlots)
    {
        _gameStats = gameStats;
        _enemyCounter = counter;
        _takeDamageView = takeDamageView;
        _userCharacter = userCharacter;
        _unitSpawner = unitSpawner;
        _quickSlots = quickSlots;
    }
}
