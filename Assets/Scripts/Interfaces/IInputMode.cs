using System;
using UnityEngine;

public interface IInputMode
{
    public event Action<float, float> OnRotated;

    public event Action<Vector3> OnMove;

    public event Action OnShot;

    public event Action<bool> OnAimed;
    public event Action<bool> OnSprint;

    public void CheckInput();
}
