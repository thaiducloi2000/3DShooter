using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] private Vector2 move;
    [SerializeField] private Vector2 look;
    [SerializeField] private bool jump;
    [SerializeField] private bool sprint;

    [Header("Movement Settings")]
    [SerializeField] private bool analogMovement;

    [Header("Mouse Cursor Settings")]
    [SerializeField] private bool cursorLocked = true;
    [SerializeField] private bool cursorInputForLook = true;

    public Vector2 Move => move;
    public Vector2 Look => look;
    public bool Jump => jump;
    public bool Sprint => sprint;
    public bool AnalogMovement => analogMovement;

    public bool CursorLocked => cursorLocked;
    public bool CursorInputForLook => cursorInputForLook;

    #region Public Set Method With Condition
    public void DoNotJump()
    {
        jump = false;
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
    #endregion

    #region Private Set Method
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
}
