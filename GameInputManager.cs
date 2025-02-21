using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameInputManager : MonoBehaviour
{
    public event EventHandler OnShootKey;
    public event EventHandler OnShootMouse;
    public event EventHandler OnPause;

    public class OnPositionChangeEventArgs : EventArgs
    {
        public float value;
    }

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.ShootKey.performed += ShootKey_Performed;
        playerInputActions.Player.ShootMouse.performed += ShootMouse_Performed;
        playerInputActions.UI.Pause.performed += Pause_Performed;
    }

    private void Start()
    {
        pauseButton.onClick.AddListener(() =>
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        });
        resumeButton.onClick.AddListener(() =>
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        });
    }

    public Vector2 GetMoveDirection()
    {
        float axisDir = playerInputActions.Player.Move.ReadValue<float>();
        Vector2 moveDir = new Vector2(axisDir, 0).normalized;
        return moveDir;
    }

    private void ShootKey_Performed(InputAction.CallbackContext context)
    {
        OnShootKey?.Invoke(this, EventArgs.Empty);
    }

    private void ShootMouse_Performed(InputAction.CallbackContext context)
    {
        OnShootMouse?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_Performed(InputAction.CallbackContext context)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    public void ResetSubscribers()
    {
        playerInputActions.Player.ShootKey.performed -= ShootKey_Performed;
        playerInputActions.Player.ShootMouse.performed -= ShootMouse_Performed;
        playerInputActions.UI.Pause.performed -= Pause_Performed;
    }
}
