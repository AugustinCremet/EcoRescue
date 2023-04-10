using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject _StartMenuCanvas;
    [SerializeField] private GameObject _MainMenuCanvas;
    [SerializeField] private GameObject _CreditsCanvas;
    [SerializeField] private GameObject _LoadingCanvas;
    [SerializeField] private GameObject _TransitionCanvas;
    [SerializeField] private GameObject _HUDCanvas;
    [SerializeField] private GameObject _HUDTutoCanvas;
    [SerializeField] private GameObject _PauseCanvas;
    [SerializeField] private GameObject _OptionsCanvas;
    [SerializeField] private GameObject _InventoryCanvas;
    [SerializeField] private GameObject _WinConditionCanvas;
    [SerializeField] private GameObject _LoseConditionCanvas;
    [Space(10)]

    [Header("Options")]
    [SerializeField] private GameObject _ControlsCanvas;
    [SerializeField] private GameObject _GameplayCanvas;
    [SerializeField] private GameObject _AudioCanvas;
    [Space(10)]

    [Header("Inventory")]
    [SerializeField] private GameObject _InventoryTabCanvas;
    [SerializeField] private GameObject _SkillTreeTabCanvas;
    [SerializeField] private GameObject _MapTabCanvas;
    [SerializeField] private GameObject _AchievementsTabCanvas;
    [SerializeField] private GameObject _MerchantShopCanvas;
    [SerializeField] private GameObject _TrainerShopCanvas;
    [Space(10)]

    [Header("Tutorial")]
    [SerializeField] private GameObject _TutorialCanvas;
    [SerializeField] private GameObject _TutorialChoiceCanvas;

    [Header("Secret Zone")]
    [SerializeField] private GameObject _SecretZone;

    [Header("Cursor")]
    [SerializeField] private GameObject _CursorCanvas;

    private bool isGamemode = false;
    [HideInInspector] public bool isDoingTutorial = false;

    private Animator _transitionAnimator;

    public GameObject SecretZone => _SecretZone;

    [Header("Others")]

    [SerializeField] private PowerObject _powerObject;
    [SerializeField] private StaminaSkill _staminaSkill;

    public bool GodMode { get; set; }

    private void Awake()
    {
        _transitionAnimator = _TransitionCanvas.GetComponent<Animator>();
    }

    private void ShowCanvas(GameObject canvas)
    {
        canvas.SetActive(true);
    }

    private void HideCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
    }

    //--- TRANSITIONS FUNCTIONS ---//
    public void StartMenuTransition()
    {
        isGamemode = false;

        HideCanvas(_StartMenuCanvas);
        ShowCanvas(_MainMenuCanvas);
    }

    public void PauseMenuTransition()
    {
        Time.timeScale = 0f;
        HideCanvas(_HUDCanvas);
        HideCanvas(_TutorialCanvas);
        ShowCanvas(_PauseCanvas);
        ShowCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }

    public void InventoryMenuTransition()
    {
        Time.timeScale = 0f;
        HideCanvas(_HUDCanvas);
        HideCanvas(_TutorialCanvas);
        ShowCanvas(_InventoryCanvas);
        ShowCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }
    public void WinTransition()
    {
        Time.timeScale = 0f;
        HideCanvas(_HUDCanvas);
        ShowCanvas(_WinConditionCanvas);
        ShowCanvas(_CursorCanvas);
    }

    public void LoseTransition()
    {
        Time.timeScale = 0f;
        HideCanvas(_HUDCanvas);
        ShowCanvas(_LoseConditionCanvas);
        ShowCanvas(_CursorCanvas);
    }

    public void RetryTransition()
    {
        Time.timeScale = 1f;
        ButtonNewGame();
    }

    public void LoadingTransition()
    {
        HideCanvas(_InventoryCanvas);
        HideCanvas(_LoadingCanvas);
        HideCanvas(_CursorCanvas);
        HideCanvas(_TrainerShopCanvas);
        HideCanvas(_SkillTreeTabCanvas);
    }
    
    public void OpenHUD()
    {
        ShowCanvas(_HUDCanvas);
        EventManager.TriggerEvent(Events.RESET, new Dictionary<string, object> { { "reset", true } });
    }

    public void OpenQuickUse()
    {
        ShowCanvas(_HUDCanvas);
    }

    public void CloseQuickUse()
    {
        HideCanvas(_HUDCanvas);
    }

    public void OpenCredits()
    {
        ShowCanvas(_CreditsCanvas);
    }
    public void CloseCredits()
    {
        HideCanvas(_CreditsCanvas);
    }

    public void SceneTransition(Dictionary<string, object> message)
    {
        _transitionAnimator.enabled = true;
        HideCanvas(_HUDCanvas);
    }

    public void FadeOut(Dictionary<string, object> message)
    {
        _transitionAnimator.SetBool("FadeOut", true);
        ShowCanvas(_HUDCanvas);
    }

    public void MerchantTransition(GameObject merchantCanvas)
    {
        Time.timeScale = 0f;
        //_merchantCanvas = merchantCanvas;
        HideCanvas(_HUDCanvas);
        ShowCanvas(_MerchantShopCanvas);
        ShowCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }
    public void TrainerTransition(GameObject trainerCanvas)
    {
        Time.timeScale = 0f;
        //_trainerCanvas = trainerCanvas;
        HideCanvas(_HUDCanvas);
        ShowCanvas(_TrainerShopCanvas);
        ShowCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }

    private void StartHUDTuto(Dictionary<string, object> message)
    {
        ShowCanvas(_HUDTutoCanvas);
        _HUDTutoCanvas.GetComponent<TutorialManager>().StartTuto();
    }

    //Coroutine for controling sound

    private IEnumerator PlayTutorialSound()
    {
        yield return new WaitForSeconds(0.8f);
        
        if(_TutorialChoiceCanvas.activeSelf)
            FindObjectOfType<SpeechManager>().PlaySpeech("tutorial");
    }
    

    //--- BUTTONS FUNCTIONS ---//
    // > Main Menu
    public void ButtonOpenTutorialPopup()
    {
        ShowCanvas(_TutorialChoiceCanvas);

        StartCoroutine(PlayTutorialSound());
    }
    public void ButtonCloseTutorialPopup()
    {
        FindObjectOfType<SpeechManager>().StopSpeech("tutorial");
        HideCanvas(_TutorialChoiceCanvas);
    }

    public void ButtonTutorialYes()
    {
        isDoingTutorial = true;
        _TutorialCanvas.GetComponent<UITutorial>().ResetValues();
    }
    public void ButtonTutorialNo()
    {
        isDoingTutorial = false;
    }

    public void ButtonNewGame()
    {
        isGamemode = true;

        HideCanvas(_TutorialChoiceCanvas);
        HideCanvas(_MainMenuCanvas);
        HideCanvas(_LoseConditionCanvas);
        HideCanvas(_WinConditionCanvas);
        ShowCanvas(_LoadingCanvas);
        ShowCanvas(_InventoryCanvas);
        ShowCanvas(_SkillTreeTabCanvas);
        ShowCanvas(_TrainerShopCanvas);

        _powerObject.ResetPowerPotion();

        EventManager.TriggerEvent(Events.RESET, new Dictionary<string, object> { { "reset", true } });
        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", false } });

        EventManager.TriggerEvent(Events.START_NEW_GAME, new Dictionary<string, object> { { "loadTutorial", isDoingTutorial }});
    }

    public void ButtonOptions()
    {
        ShowCanvas(_OptionsCanvas);
        ShowCanvas(_CursorCanvas);

        if (isGamemode) { FindObjectOfType<Player>().enabled = false; }
    }

    public void ButtonQuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // > Pause Menu
    public void ButtonResumeGame()
    {
        Time.timeScale = 1f;
        HideCanvas(_PauseCanvas);
        HideCanvas(_MerchantShopCanvas);
        HideCanvas(_TrainerShopCanvas);
        ShowCanvas(_HUDCanvas);

        if(isDoingTutorial)
            ShowCanvas(_TutorialCanvas);
        HideCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", false } });
    }

    public void ButtonExitCurrentGame()
    {
        Time.timeScale = 1f;
        isGamemode = false;

        SceneManager.UnloadSceneAsync(GameManager.instance.ActiveDungeon.Name);
        SceneManager.UnloadSceneAsync(GameManager.instance.ActiveDungeonManager.ActiveRoom.Name);
        
        GetComponent<AudioListener>().enabled = true;

        HideCanvas(_HUDCanvas);
        HideCanvas(_PauseCanvas);
        ShowCanvas(_MainMenuCanvas);
        ShowCanvas(_CursorCanvas);
    }
    public void ButtonRetry()
    {
        _TutorialCanvas.GetComponent<UITutorial>().ResetValues();
        HideCanvas(_LoseConditionCanvas);
        HideCanvas(_WinConditionCanvas);
        HideCanvas(_PauseCanvas);

        isDoingTutorial = DiedInTutorial();

        ButtonExitCurrentGame();
        ButtonNewGame();
    }

    // > Options Menu
    public void ButtonBackFromOptions()
    {
        HideCanvas(_OptionsCanvas);
        if (isGamemode) { FindObjectOfType<Player>().enabled = true; }
    }
    public void ButtonOpenControls()
    {
        HideCanvas(_GameplayCanvas);
        HideCanvas(_AudioCanvas);
        ShowCanvas(_ControlsCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }
    public void ButtonOpenGameplay()
    {
        HideCanvas(_AudioCanvas);
        ShowCanvas(_GameplayCanvas);
    }
    public void ButtonOpenAudio()
    {
        ShowCanvas(_AudioCanvas);
    }

    // > Inventory Menu
    public void ButtonResumeFromInventory()
    {
        Time.timeScale = 1f;
        HideCanvas(_InventoryCanvas);
        ShowCanvas(_HUDCanvas);
        if (isDoingTutorial)
            ShowCanvas(_TutorialCanvas);
        HideCanvas(_CursorCanvas);

        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", false} });
    }
    
    public void ButtonOpenInventory()
    {
        HideCanvas(_SkillTreeTabCanvas);
        HideCanvas(_MapTabCanvas);
        HideCanvas(_AchievementsTabCanvas);
        ShowCanvas(_InventoryTabCanvas);
        ShowCanvas(_CursorCanvas);
    }
    
    public void ButtonOpenSkillTree()
    {
        HideCanvas(_MapTabCanvas);
        HideCanvas(_AchievementsTabCanvas);
        ShowCanvas(_SkillTreeTabCanvas);
    }
    
    public void ButtonOpenMap()
    {
        HideCanvas(_AchievementsTabCanvas);
        ShowCanvas(_MapTabCanvas);
    }
    
    public void ButtonOpenAchievements()
    {
        ShowCanvas(_AchievementsTabCanvas);
    }

    //-----Event-----

    private void PlayerWin(Dictionary<string, object> message)
    {
        WinTransition();
    }

    private void PlayerLose(Dictionary<string, object> message)
    {
        LoseTransition();
    }

    private bool DiedInTutorial()
    {
        return GameManager.instance.ActiveDungeonManager.ActiveRoom == GameManager.instance.ActiveDungeon.TutorialRoom;
    }


    private void OnEnable()
    {
        EventManager.StartListening(Events.WIN, PlayerWin);
        EventManager.StartListening(Events.PLAYER_DIED, PlayerLose);
        EventManager.StartListening(Events.SWITCH_ROOM, SceneTransition);
        EventManager.StartListening(Events.FADE_OUT, FadeOut);
        EventManager.StartListening(Events.START_HUD_TUTO, StartHUDTuto);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.WIN, PlayerWin);
        EventManager.StopListening(Events.PLAYER_DIED, PlayerLose);
        EventManager.StopListening(Events.SWITCH_ROOM, SceneTransition);
        EventManager.StopListening(Events.FADE_OUT, FadeOut);
        EventManager.StopListening(Events.START_HUD_TUTO, StartHUDTuto);
    }
}
