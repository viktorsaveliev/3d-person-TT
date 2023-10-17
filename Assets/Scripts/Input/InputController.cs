using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    private IInputMode _inputMode;

    private void Update()
    {
        _inputMode?.CheckInput();
    }

    [Inject]
    public void Construct(IInputMode inputMode)
    {
        _inputMode = inputMode;
    }
}
