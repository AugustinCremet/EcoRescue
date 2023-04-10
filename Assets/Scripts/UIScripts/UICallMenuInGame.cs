using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UICallMenuInGame : MonoBehaviour
{
    private UIManager _uiManager;
    private PlayerInputs _playerInputs;

    private void Start()
    {
        _uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;

        _playerInputs = GameManager.instance.Inputs;

        _playerInputs.Menu.Inventory.performed += OpenInventoryMenu;
        _playerInputs.Menu.Pause.performed += OpenPauseMenu;
    }

    private void OnDisable()
    {
        _playerInputs.Menu.Disable();
    }

    private void OpenInventoryMenu(InputAction.CallbackContext context)
    {
        FindObjectOfType<SpeechManager>().PlaySpeech("inventory");
        
        _uiManager.InventoryMenuTransition();
    }

    private void OpenPauseMenu(InputAction.CallbackContext context)
    {
        FindObjectOfType<SpeechManager>().PlaySpeech("pause");
        
        _uiManager.PauseMenuTransition();
    }
}
