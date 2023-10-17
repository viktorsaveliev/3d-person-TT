using UnityEngine;
using Zenject;

public class GameInstance : MonoInstaller
{
    private readonly IInputMode _inputMode = new PCInput();

    public override void InstallBindings()
    {
        Container.Bind<IInputMode>().FromInstance(_inputMode);
    }
}
