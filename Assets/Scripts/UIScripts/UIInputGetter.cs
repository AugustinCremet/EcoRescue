using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputGetter : MonoBehaviour
{
    private PlayerInputs _playerInputs;

    private bool _enable;

    // Start is called before the first frame update
    void Start()
    {
        _playerInputs = new PlayerInputs();

        _playerInputs.Menu.Inventory.performed += getInput;
        _playerInputs.Menu.Pause.performed += getInput;
        _playerInputs.Movement.Walk.performed += getInput;
        _playerInputs.Movement.Dash.performed += getInput;
        _playerInputs.Skill.HeavyAttack.performed += getInput;
        _playerInputs.Skill.LightAttack.performed += getInput;
        _playerInputs.Menu.KeyDetect.started += getInput;
    }

    private void getInput(InputAction.CallbackContext context)
    {
        Debug.Log("yes");
    }
}
