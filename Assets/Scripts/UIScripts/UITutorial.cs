using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text _objective1;
    [SerializeField] private TMP_Text _objective2;
    [SerializeField] private TMP_Text _objective3;
    [SerializeField] private TMP_Text _objective4;

    private int _currentObjective = 1;

    private int _dashCount = 0;
    [SerializeField] private int _dashToDo = 3;
    private int _lightAttackCount = 0;
    [SerializeField] private int _lightAttackToDo = 5;
    private int _heavyAttackCount = 0;
    [SerializeField] private int _heavyAttackToDo = 2;
    private int _projectileCount = 0;
    [SerializeField] private int _projectileToDo = 3;
    private int _chargeAttackCount = 0;
    [SerializeField] private int _chargeAttackToDo = 1;
    void Update()
    {
        switch (_currentObjective)
        {
            case 1:
                MainObjective();
                FirstObjective();
                break;
            case 2:
                MainObjective();
                SecondObjective();
                break;
            case 3:
                MainObjective();
                ThirdObjective();
                break;
            case 4:
                MainObjective();
                break;
            case 5:
                FinalObjective();
                break;
            default:
                break;
        }
    }

    private void MainObjective()
    {
        if (GameManager.instance.ActiveDungeonManager == null)
            return;

        int enemyCount = GameManager.instance.ActiveDungeonManager.ActiveRoom.NbOfEnemiesAlive;

        //if (enemyCount <= 0 && _currentObjective == 4)
        //{
        //    _currentObjective = 5;
        //}

        if (_currentObjective < 5)
        {
            if(enemyCount > 0)
            {
                _objective1.text = $"Eliminate all the enemies        \n" +
                                   $"to unlock the next room: {enemyCount} left";
                _objective1.gameObject.SetActive(true);
            }
            else
            {
                if(_currentObjective < 4)
                {
                    _objective1.text = $"All enemies have been eliminated,\n" +
                                       $"finish the other objectives.";
                    _objective1.color = Color.green;
                }
                else
                {
                    _objective1.text = $"All quests have been completed,\n" +
                                       $"proceed to the next room!!!";
                    _objective1.color = Color.green;
                }
            }         
        }
    }
    private void FirstObjective()
    {
        _objective2.text = "Move your character";
        _objective2.gameObject.SetActive(true);
        _objective3.text = $"Dash {_dashToDo} times: {_dashCount} / {_dashToDo}";
        _objective3.gameObject.SetActive(true);
    }

    private void FirstObjectiveProgress(Dictionary<string, object> message)
    {
        if (_currentObjective != 1)
            return;

        bool hasWalked = false;

        if ((string)message["movement"] == "walk")
        {
            _objective2.color = Color.green;
            hasWalked = true;
        }
        else if ((string)message["movement"] == "dash")
        {
            if (_dashCount < _dashToDo)
                _dashCount++;
            if (_dashCount == _dashToDo)
                _objective3.color = Color.green;
        }

        if (hasWalked && _dashCount >= _dashToDo)
        {
            _currentObjective = 2;
            ResetObjectiveColor();
        }
    }

    private void SecondObjective()
    {
        _objective2.text = $"Do {_lightAttackToDo} light attacks: {_lightAttackCount} / {_lightAttackToDo}";
        _objective2.gameObject.SetActive(true);
        _objective3.text = $"Do {_heavyAttackToDo} heavy attacks: {_heavyAttackCount} / {_heavyAttackToDo}";
        _objective2.gameObject.SetActive(true);

    }

    private void SecondObjectiveProgress(Dictionary<string, object> message)
    {
        if (_currentObjective != 2)
            return;

        if ((string)message["attack"] == "light")
        {
            if (_lightAttackCount < _lightAttackToDo)
                _lightAttackCount++;
            if (_lightAttackCount == _lightAttackToDo)
                _objective2.color = Color.green;
        }
        else if ((string)message["attack"] == "heavy")
        {
            if (_heavyAttackCount < _heavyAttackToDo)
                _heavyAttackCount++;
            if (_heavyAttackCount == _heavyAttackToDo)
                _objective3.color = Color.green;
        }

        if (_lightAttackCount >= _lightAttackToDo && _heavyAttackCount >= _heavyAttackToDo)
        {
            ResetObjectiveColor();
            _currentObjective = 3;
        }
    }

    private void ThirdObjective()
    {
        _objective2.text = $"Shoot {_projectileToDo} projectiles: {_projectileCount} / {_projectileToDo}";
        _objective3.text = $"Do {_chargeAttackToDo} charge attack: {_chargeAttackCount} / {_chargeAttackToDo}";
    }

    private void ThirdObjectiveProgress(Dictionary<string, object> message)
    {
        if (_currentObjective != 3)
            return;

        if ((string)message["attack"] == "projectile")
        {
            if (_projectileCount < _projectileToDo)
                _projectileCount++;
            if (_projectileCount == _projectileToDo)
                _objective2.color = Color.green;
        }
        else if ((string)message["attack"] == "charge")
        {
            if (_chargeAttackCount < _chargeAttackToDo)
                _chargeAttackCount++;
            if (_chargeAttackCount == _chargeAttackToDo)
                _objective3.color = Color.green;
        }

        if (_projectileCount >= _projectileToDo && _chargeAttackCount >= _chargeAttackToDo)
        {
            _objective2.gameObject.SetActive(false);
            _objective3.gameObject.SetActive(false);
            ResetObjectiveColor();
            _currentObjective = 4;
        }
    }

    private void FinalObjective()
    {
        _objective1.text = "Go to the exit";
    }

    private void CloseUI(Dictionary<string, object> message)
    {
        var manager = FindObjectOfType(typeof(UIManager)) as UIManager;
        manager.isDoingTutorial = false;
        gameObject.SetActive(false);
    }

    private void ResetObjectiveColor()
    {
        _objective2.color = Color.red;
        _objective3.color = Color.red;
        _objective4.color = Color.red;
    }

    public void ResetValues()
    {
        _currentObjective = 1;
        _dashCount = 0;
        _lightAttackCount = 0;
        _heavyAttackCount = 0;
        _projectileCount = 0;
        _chargeAttackCount = 0;
        ResetObjectiveColor();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.StartListening(Events.PLAYER_MOVEMENT, FirstObjectiveProgress);
        EventManager.StartListening(Events.PLAYER_BASIC_ATTACK, SecondObjectiveProgress);
        EventManager.StartListening(Events.PLAYER_CHARGE_ATTACK, ThirdObjectiveProgress);
        EventManager.StartListening(Events.SWITCH_ROOM, CloseUI);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PLAYER_MOVEMENT, FirstObjectiveProgress);
        EventManager.StopListening(Events.PLAYER_BASIC_ATTACK, SecondObjectiveProgress);
        EventManager.StopListening(Events.PLAYER_CHARGE_ATTACK, ThirdObjectiveProgress);
        EventManager.StopListening(Events.SWITCH_ROOM, CloseUI);
    }
}
