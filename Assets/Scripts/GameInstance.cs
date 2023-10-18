using UnityEngine;
using Zenject;

public class GameInstance : MonoInstaller
{
    [SerializeField] private UserCharacter _userUnit;

    private readonly IInputMode _inputMode = new PCInput();

    public override void InstallBindings()
    {
        Container.Bind<IInputMode>().FromInstance(_inputMode).AsSingle();
        Container.Bind<UserCharacter>().FromInstance(_userUnit).AsSingle();
    }
}
