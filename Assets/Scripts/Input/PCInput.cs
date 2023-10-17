using System;
using UnityEngine;

public class PCInput : IInputMode
{
    public event Action<float, float> OnRotated;

    public event Action<Vector3> OnMove;

    public event Action OnShot;

    public event Action<bool> OnAimed;
    public event Action<bool> OnSprint;

    public void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnAimed?.Invoke(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnAimed?.Invoke(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnShot?.Invoke();
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        OnRotated?.Invoke(mouseX, mouseY);

        Vector3 moveDirection = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        OnMove?.Invoke(moveDirection);
    }
}
