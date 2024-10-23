using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] private Vector2 move;
    [SerializeField] private Vector2 look;
    [SerializeField] private bool jump;
    [SerializeField] private bool sprint;

    [Header("Camera Input Values")]
    [SerializeField] private bool isAim;
    [SerializeField] private bool isShoot;

    [Header("Movement Settings")]
    [SerializeField] private bool analogMovement;

    [Header("Mouse Cursor Settings")]
    [SerializeField] private bool cursorLocked = true;
    [SerializeField] private bool cursorInputForLook = true;

    private UnityAction<bool> isAimCallBack;
    private UnityAction<bool> isShootCallBack;

    public Vector2 Move => move;
    public Vector2 Look => look;
    public bool Jump => jump;
    public bool Sprint => sprint;
    public bool AnalogMovement => analogMovement;

    public bool CursorLocked => cursorLocked;
    public bool CursorInputForLook => cursorInputForLook;

    private void OnDisable()
    {
        ClearAllCallBack();
    }

    #region Public Set Method With Condition
    public void DoNotJump()
    {
        jump = false;
    }

    public void AssignOnAimCallBack(UnityAction<bool> callback)
    {
        isAimCallBack += callback;
    }

    public void AssignOnShootCallBack(UnityAction<bool> callback)
    {
        isShootCallBack += callback;
    }

    public void RemoveAimCallBack(UnityAction<bool> callback)
    {
        isAimCallBack -= callback;
    }

    public void RemoveShootallBack(UnityAction<bool> callback)
    {
        isShootCallBack -= callback;
    }

    private void ClearAllCallBack()
    {
        isAimCallBack = null;
        isShootCallBack = null;
    }
    #endregion
    #region Public Set Method Call Back From New Input System
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (CursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    #endregion

    #region Private Set Method Player Input
    private void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    private void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    private void JumpInput(bool newJumpState)
    {
        //jump = newJumpState;
    }

    private void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(CursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    #endregion

    #region Private Set Camera Invoke
    private void AimInput(bool newStateAim)
    {
        if (isAim == newStateAim) return;
        isAim = newStateAim;
        isAimCallBack?.Invoke(isAim);
        //jump = newJumpState;
    }
    private void ShootInput(bool newShootState)
    {
        isShoot = newShootState;
        isShootCallBack?.Invoke(isShoot);
    }

    #endregion
}
