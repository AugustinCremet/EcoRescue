using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    private Mouse _virtualMouse;
    private Mouse _currentMouse;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private Canvas _GamepadCanvas;
    [SerializeField] private RectTransform _GamepadCanvasTransform;
    [SerializeField] private RectTransform _cursorTransform;
    [SerializeField] private float _cursorSpeed = 1000f;
    [SerializeField] private float _cursorIconPadding = 50f;

    private Camera _camera;

    private bool _previousMouseState;

    private string _previousControlScheme = "";

    private const string gamepadScheme = "Gamepad";
    private const string keyboardMouseScheme = "KeyboardMouse";

    public bool _isUsingKeyboard = true;

    private void OnEnable()
    {
        _currentMouse = Mouse.current;

        InputDevice virtualMouseInputDevice = InputSystem.GetDevice("VirtualMouse");
        
        if (virtualMouseInputDevice == null)
        {
            _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouseInputDevice.added)
        {
            _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else
        {
            _virtualMouse = (Mouse)virtualMouseInputDevice;
        }

        if (_cursorTransform != null)
        {
            Vector2 position = _cursorTransform.anchoredPosition;
            InputState.Change(_virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        _playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if(_virtualMouse != null && _virtualMouse.added) InputSystem.RemoveDevice(_virtualMouse);

        InputSystem.onAfterUpdate -= UpdateMotion;
        _playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if(_virtualMouse == null) return;

        Vector2 stickValue;
        Vector2 currentPosition;
        Vector2 newPosition;

        if (!_isUsingKeyboard && Gamepad.current != null)
        {
            stickValue = Gamepad.current.leftStick.ReadValue();
            stickValue *= _cursorSpeed * Time.unscaledDeltaTime;

            currentPosition = _virtualMouse.position.ReadValue();
            newPosition = currentPosition + stickValue;
        }
        else
        {
            stickValue = Input.mousePosition;
            newPosition = stickValue;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, _cursorIconPadding, Screen.width - _cursorIconPadding);
        newPosition.y = Mathf.Clamp(newPosition.y, _cursorIconPadding, Screen.height - _cursorIconPadding);

        InputState.Change(_virtualMouse, newPosition);
        InputState.Change(_virtualMouse.delta, stickValue);

        bool aButtonIsPressed = false;
        if (Gamepad.current != null)
            aButtonIsPressed = Gamepad.current.aButton.IsPressed();

        if (_previousMouseState != aButtonIsPressed)
        {
            _virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(_virtualMouse, mouseState);
            _previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 pos)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_GamepadCanvasTransform, pos, _GamepadCanvas.renderMode 
            == RenderMode.ScreenSpaceOverlay ? null : _camera, out anchoredPosition);

        _cursorTransform.anchoredPosition = anchoredPosition;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if(_playerInput.currentControlScheme == keyboardMouseScheme && _previousControlScheme != keyboardMouseScheme)
        {
            _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
            _previousControlScheme = keyboardMouseScheme;
            _isUsingKeyboard = true;
        }
        else if (_playerInput.currentControlScheme == gamepadScheme && _previousControlScheme != gamepadScheme)
        {
            InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);
            InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
            AnchorCursor(_currentMouse.position.ReadValue());
            _previousControlScheme = gamepadScheme;
            _isUsingKeyboard = false;
        }
    }
}
