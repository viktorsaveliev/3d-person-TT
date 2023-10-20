using System;
using UnityEngine;

public class KeyboardInput : IInputMode
{
    public event Action<int> OnSelectWeapon;
    public event Action<float, float> OnRotated;
    public event Action<Vector3> OnMove;

    public event Action OnReloadWeapon;
    public event Action OnShot;
    public event Action OnOpenInventory;
    public event Action OnInteraction;

    public event Action<bool> OnAimed;
    public event Action<bool> OnSprint;

    public void CheckInput()
    {
        CheckWeaponSelectionInput();
        CheckAimInput();
        CheckSprintInput();
        CheckReloadInput();
        CheckInventoryInput();
        CheckInteractionInput();
        CheckShootInput();
        CheckRotationInput();
        CheckMovementInput();
    }

    private void CheckWeaponSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnSelectWeapon?.Invoke(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSelectWeapon?.Invoke(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnSelectWeapon?.Invoke(3);
        }
    }

    private void CheckAimInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnAimed?.Invoke(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnAimed?.Invoke(false);
        }
    }

    private void CheckSprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(false);
        }
    }

    private void CheckReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnReloadWeapon?.Invoke();
        }
    }

    private void CheckInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnOpenInventory?.Invoke();
        }
    }

    private void CheckInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteraction?.Invoke();
        }
    }

    private void CheckShootInput()
    {
        if (Input.GetMouseButton(0))
        {
            OnShot?.Invoke();
        }
    }

    private void CheckRotationInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        OnRotated?.Invoke(mouseX, mouseY);
    }

    private void CheckMovementInput()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        OnMove?.Invoke(moveDirection);
    }
}
