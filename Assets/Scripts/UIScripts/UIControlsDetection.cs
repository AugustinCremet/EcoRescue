using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UIControlsDetection : MonoBehaviour
{
    private ControlChangeInGame _getControl;

    [SerializeField] private Image _buttonImage;
    [SerializeField] private Text[] _buttonText;
    [SerializeField] private bool _changeTextOnly;

    [Header("Gamepad Mode")]
    [SerializeField] private Sprite _gamepadImage;
    [SerializeField] private string[] _textGamepad;
    [Space(10)]

    [Header("Keyboard and Mouse Mode")]
    [SerializeField] private Sprite _keyboardImage;
    [SerializeField] private string[] _textKeyboard;


    private void Awake()
    {
        _getControl = FindObjectOfType(typeof(ControlChangeInGame)) as ControlChangeInGame;
        
        if(_getControl.IsUsingKeyboard)
        {
            SetToKeyboard();
        }
        else
        {
            SetToGamepad();
        }
    }

    private void OnEnable()
    {
        ChangeDisplay();
    }

    private void OnDisable()
    {
        ChangeDisplay();
    }

    private void ChangeDisplay()
    {
        InputSystem.onActionChange += (obj, change) =>
        {
            if (change == InputActionChange.ActionPerformed)
            {
                var inputAction = (InputAction)obj;
                var lastControl = inputAction.activeControl;
                var lastDevice = lastControl.device;

                if (lastDevice.displayName == "Keyboard" || lastDevice.displayName == "Mouse")
                {
                    SetToKeyboard();
                }
                else
                {
                    SetToGamepad();
                }
            }
        };
    }

    private void SetToKeyboard()
    {
        ChangeText(_textKeyboard);
        if (_changeTextOnly == false)
        {
            ChangeSprite(_keyboardImage);
        }
    }
    private void SetToGamepad()
    {
        ChangeText(_textGamepad);
        if (_changeTextOnly == false)
        {
            ChangeSprite(_gamepadImage);
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        _buttonImage.sprite = sprite;
    }

    private void ChangeText(string[] text)
    {
        for (int i = 0; i < _buttonText.Length; i++)
        {
            _buttonText[i].text = text[i].ToString();
        }
    }
}