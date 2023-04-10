using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlChangeInGame : MonoBehaviour
{
    private static ControlChangeInGame _controlChangeInGame;
    private bool _isUsingKeyboard;
    public bool IsUsingKeyboard { get { return _isUsingKeyboard; } }
    public static ControlChangeInGame Instance
    {
        get
        {
            if (!_controlChangeInGame)
            {
                _controlChangeInGame = FindObjectOfType(typeof(ControlChangeInGame)) as ControlChangeInGame;

                if (!_controlChangeInGame)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    DontDestroyOnLoad(_controlChangeInGame);
                }
            }
            return _controlChangeInGame;
        }
    }
    void Start()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;
                //Debug.Log($"device: {lastDevice.displayName}");
                if (lastDevice.displayName == "Keyboard" || lastDevice.displayName == "Mouse")
                {
                    _isUsingKeyboard = true;
                }
                else
                {
                    _isUsingKeyboard = false;
                }
            }
        };
    }
}
