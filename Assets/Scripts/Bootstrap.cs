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

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _unitSpawner.Init();
        _userCharacter.Init();
        _enemyCounter.Init();
        _gameStats.Init();
        _takeDamageView.Init();
        _quickSlots.Init();
    }

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
