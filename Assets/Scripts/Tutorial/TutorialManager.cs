using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<HUDDescription> _tutoStep = new List<HUDDescription>();
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private GameObject _previousText;
    [SerializeField] private GameObject _nextText;
    [SerializeField] private GameObject _SkipText;
    [SerializeField] private string _endText = "Continue to Game";

    [SerializeField] private GameObject _tutorialObjectivesCanvas;

    private int _stepIdx = 0;
    private PlayerInputs _playerInputs;

    private void Start()
    {
        _playerInputs = GameManager.instance.Inputs;
        _playerInputs.Tutorial.Enable();

        _playerInputs.Tutorial.Next.started += OnNext;
        _playerInputs.Tutorial.Next.canceled += OnNext;

        _playerInputs.Tutorial.Previous.started += OnPrevious;
        _playerInputs.Tutorial.Previous.canceled += OnPrevious;
        
        _playerInputs.Tutorial.Skip.started += OnSkip;
        _playerInputs.Tutorial.Skip.canceled += OnSkip;
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (_stepIdx == _tutoStep.Count - 1)
            {
                EndTutorial();
                return;
            }

            _previousText.SetActive(true);
            _tutoStep[_stepIdx].DeactivateCover();
            _stepIdx++;

            if (_stepIdx == _tutoStep.Count - 1)
            {
                var text = _nextText.GetComponentInChildren<TMP_Text>();
                text.color = Color.green;
                text.text = _endText;
            }

            _tutoStep[_stepIdx].ActivateCover();
            _descriptionText.text = _tutoStep[_stepIdx].Description;
        }
    }

    private void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            _nextText.SetActive(true);

            var nextText = _nextText.GetComponentInChildren<TMP_Text>();
            nextText.color = Color.white;
            nextText.text = "Next";

            if (_stepIdx > 0)
            {
                _tutoStep[_stepIdx].DeactivateCover();
                _stepIdx--;
            }
            else return;

            if (_stepIdx == 0)
                _previousText.SetActive(false);

            _tutoStep[_stepIdx].ActivateCover();
            _descriptionText.text = _tutoStep[_stepIdx].Description;
        }
    }

    private void OnSkip(InputAction.CallbackContext context) 
    {
        EndTutorial();
    }

    public void StartTuto()
    {
        _SkipText.SetActive(true);
        _nextText.SetActive(true);
        _tutoStep[_stepIdx].ActivateCover();
        _descriptionText.text = _tutoStep[_stepIdx].Description;
    }

    private void EndTutorial()
    {
        GameManager.instance.EnablePlayerInputs();
        _tutoStep[_stepIdx].DeactivateCover();
        _tutorialObjectivesCanvas.SetActive(true);
        this.gameObject.SetActive(false);

        _SkipText.SetActive(false);
    }

    private void OnEnable()
    {
        if (_playerInputs != null) 
            _playerInputs.Tutorial.Enable();

        _stepIdx = 0;
        var nextText = _nextText.GetComponentInChildren<TMP_Text>();
        nextText.color = Color.white;
        nextText.text = "Next";
        _previousText.SetActive(false);
    }

    private void OnDisable()
    {
        _playerInputs.Tutorial.Disable();
    }
}
