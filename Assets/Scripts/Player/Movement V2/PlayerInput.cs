using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region "Event Handling"

    // delegates
    public delegate void ForwardKeyPressed();
    public delegate void ForwardKeyHold();
    public delegate void ForwardKeyReleased();
    public delegate void BackwardKeyPressed();
    public delegate void BackwardKeyHold();
    public delegate void BackwardKeyReleased();
    public delegate void RightKeyPressed();
    public delegate void RightKeyHold();
    public delegate void RightKeyReleased();
    public delegate void LeftKeyPressed();
    public delegate void LeftKeyHold();
    public delegate void LeftKeyReleased();
    public delegate void RunningKeyPressed();
    public delegate void RunningKeyHold();
    public delegate void RunningKeyReleased();
    public delegate void LeftClickPressed();
    public delegate void LeftClickHold();
    public delegate void LeftClickReleased();
    public delegate void RightClickPressed();
    public delegate void RightClickHold();
    public delegate void RightClickReleased();
    public delegate void JumpButtonPressed();
    public delegate void LockOnButtonPressed();

    // events
    public event ForwardKeyPressed OnForwardKeyPressed;
    public event ForwardKeyHold OnForwardKeyHold;
    public event ForwardKeyReleased OnForwardKeyReleased;
    public event BackwardKeyPressed OnBackwardKeyPressed;
    public event BackwardKeyHold OnBackwardKeyHold;
    public event BackwardKeyReleased OnBackwardKeyReleased;
    public event RightKeyPressed OnRightKeyPressed;
    public event RightKeyHold OnRightKeyHold;
    public event RightKeyReleased OnRightKeyReleased;
    public event LeftKeyPressed OnLeftKeyPressed;
    public event LeftKeyHold OnLeftKeyHold;
    public event LeftKeyReleased OnLeftKeyReleased;
    public event RunningKeyPressed OnRunningKeyPressed;
    public event RunningKeyHold OnRunningKeyHold;
    public event RunningKeyReleased OnRunningKeyReleased;
    public event LeftClickPressed OnLeftClickPressed;
    public event LeftClickHold OnLeftClickHold;
    public event LeftClickReleased OnLeftClickReleased;
    public event RightClickPressed OnRightClickPressed;
    public event RightClickHold OnRightClickHold;
    public event RightClickReleased OnRightClickReleased;
    public event JumpButtonPressed OnJumpButtonPressed;
    public event LockOnButtonPressed OnLockOnButtonPressed;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // we check inputs and if buttons are pressed, the appropriate events will be fired
        #region "Forward (W)"
        if (Input.GetKeyDown(KeyCode.W)) {
            OnForwardKeyPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.W)) {
            OnForwardKeyHold?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.W)) {
            OnForwardKeyReleased?.Invoke();
        }
        #endregion
        #region "Backward (S)"
        if (Input.GetKeyDown(KeyCode.S)) {
            OnBackwardKeyPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.S)) {
            OnBackwardKeyHold?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.S)) {
            OnBackwardKeyReleased?.Invoke();
        }
        #endregion
        #region "Left (A)"
        if (Input.GetKeyDown(KeyCode.A)) {
            OnLeftKeyPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.A)) {
            OnLeftKeyHold?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.A)) {
            OnLeftKeyReleased?.Invoke();
        }
        #endregion
        #region "Right (D)"
        if (Input.GetKeyDown(KeyCode.D)) {
            OnRightKeyPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.D)) {
            OnRightKeyHold?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.D)) {
            OnRightKeyReleased?.Invoke();
        }
        #endregion
        #region "Running (Left Shift)"
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            OnRunningKeyPressed?.Invoke();
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            OnRunningKeyHold?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            OnRunningKeyReleased?.Invoke();
        }
        #endregion
        #region "Left Click (Mouse1)"
        if (Input.GetMouseButtonDown(0)) {
            OnLeftClickPressed?.Invoke();
        }
        if (Input.GetMouseButton(0)) {
            OnLeftClickHold?.Invoke();
        }
        if (Input.GetMouseButtonUp(0)) {
            OnLeftClickReleased?.Invoke();
        }
        #endregion
        #region "Right Click (Mouse2)"
        if (Input.GetMouseButtonDown(1)) {
            OnRightClickPressed?.Invoke();
        }
        if (Input.GetMouseButton(1)) {
            OnRightClickHold?.Invoke();
        }
        if (Input.GetMouseButtonUp(1)) {
            OnRightClickReleased?.Invoke();
        }
        #endregion
        #region "Jump (Spacebar)"
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnJumpButtonPressed?.Invoke();
        }
        #endregion
        #region "Lock On (F)"
        if (Input.GetKeyDown(KeyCode.F)) {
            OnLockOnButtonPressed?.Invoke();
        }
        #endregion
    }
}
