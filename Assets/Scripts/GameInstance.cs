using UnityEngine;
using Zenject;

public class GameInstance : MonoInstaller
{
    [SerializeField] private UserCharacter _userUnit;

    [SerializeField] private ItemDetector _itemDetector;
    [SerializeField] private ItemInteraction _itemInteraction;

    [SerializeField] private InventoryController _inventoryController;

    private readonly IInputMode _inputMode = new KeyboardInput();

    public override void InstallBindings()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Container.Bind<IInputMode>().FromInstance(_inputMode).AsSingle();
        Container.Bind<UserCharacter>().FromInstance(_userUnit).AsSingle();

        Container.Bind<ItemDetector>().FromInstance(_itemDetector).AsSingle();
        Container.Bind<ItemInteraction>().FromInstance(_itemInteraction).AsSingle();

        Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle();
    }
}
