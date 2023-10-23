using UnityEngine;
using Zenject;

public class GameInstance : MonoInstaller
{
    [SerializeField] private UserCharacter _userUnit;

    [SerializeField] private ItemDetector _itemDetector;
    [SerializeField] private ItemInteraction _itemInteraction;

    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private TakeDamageView _takeDamageView;

    [SerializeField] private UnitFactory _unitFactory;
    [SerializeField] private UnitSpawner _unitSpawner;

    [SerializeField] private EnemyCounter _enemyCounter;

    private readonly IInputMode _inputMode = new KeyboardInput();
    private readonly ISaveData _saveData = new PlayerPrefsSaver();

    private GameStatus _gameStatus;
    private QuickSlotsController _quickSlots;
    private ITargetFinder _targetFinder;

    private void Awake()
    {
        _gameStatus = Container.Resolve<GameStatus>();
        _quickSlots = Container.Resolve<QuickSlotsController>();

        _unitSpawner.Init();
        _userUnit.Init();
        _enemyCounter.Init();
        _gameStatus.Init();
        _takeDamageView.Init();
        _quickSlots.Init();
    }

    public override void InstallBindings()
    {
        Container.Bind<IInputMode>().FromInstance(_inputMode).AsSingle();
        Container.Bind<ISaveData>().FromInstance(_saveData).AsSingle();

        Container.Bind<UserCharacter>().FromInstance(_userUnit).AsSingle();

        Container.Bind<ItemDetector>().FromInstance(_itemDetector).AsSingle();
        Container.Bind<ItemInteraction>().FromInstance(_itemInteraction).AsSingle();

        Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle();
        Container.Bind<IAmmoCounter>().FromInstance(_inventoryController).AsSingle();

        Container.Bind<UnitFactory>().FromInstance(_unitFactory).AsSingle();
        Container.Bind<UnitSpawner>().FromInstance(_unitSpawner).AsSingle();

        Container.Bind<EnemyCounter>().FromInstance(_enemyCounter).AsSingle();

        Container.Bind<TakeDamageView>().FromInstance(_takeDamageView).AsSingle();

        Container.Bind<GameStatus>().FromNew().AsSingle();
        Container.Bind<QuickSlotsController>().FromNew().AsSingle();

        _targetFinder = new RaycastTargetFinder(Camera.main);
        Container.Bind<ITargetFinder>().FromInstance(_targetFinder).AsSingle();
    }
}