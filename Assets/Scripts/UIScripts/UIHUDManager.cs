using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHUDManager : MonoBehaviour
{
    [SerializeField] private Text _coinsText;

    [SerializeField] private Transform _lifeFolder;
    [SerializeField] private GameObject _life;

    [SerializeField] private Transform _staminaFolder;
    [SerializeField] private GameObject _stamina;

    [SerializeField] private Transform _lifeFadingContainer;

    [SerializeField] private Image _imageCooldown;
    [SerializeField] private TMP_Text _textCooldown;

    [SerializeField] private GameObject _secretUI;
    [SerializeField] private GameObject[] _staminaContainer;

    //to access max value
    private Player _player;
    private bool _isChargeCooldown;
    private float _chargeCooldownTime;
    private float _chargeCooldownTimerForImage = 0f;
    private float _chargeCooldownTimerForText = 0f;

    private Queue<Image> _staminaImageQueue = new Queue<Image>();
    private bool _pauseStaminaSecret;
    private readonly Color _originalColor = new Color(0.247058824f, 0.223529412f, 0.97254902f, 1);
    private readonly Color _targetColor = new Color(0.247058824f, 0.223529412f, 0.97254902f, 0.2f);
    [Header("Time for 1 fade in or out cycle")]
    [SerializeField] private float _staminaFadeLength = 1f;
    public float StaminaFadeLength => _staminaFadeLength;
    private int _staminaFadeQty = 4;
    private Color _originalSecretUIColor = new Color(0, 1, 0.4782097f, 1);
    private Color _targetSecretUIColor = new Color(0, 1, 0.4782097f, 0);

    private bool _verifyGodModeOn = true;
    private bool _verifyGodModeOff = false;

    private int _staminaIconCount;

    //[SerializeField] PowerObject _powerObject;

    private void Awake()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
        _chargeCooldownTime = _player.ChargeAttackCooldown;
        _chargeCooldownTimerForText = _chargeCooldownTime;
    }

    private void Update()
    {
        if (_isChargeCooldown)
        {
            ApplyCooldown();
        }

        if (_player.GodMode && _verifyGodModeOn)
        {
            _verifyGodModeOn = false;
            _verifyGodModeOff = true;
            AddStamina(_player.MaxStamina, true);
            _player.StaminaCurrentCooldown = 0f;
        }
        else if (!_player.GodMode && _verifyGodModeOff)
        {
            _verifyGodModeOn = true;
            _verifyGodModeOff = false;
        }

        FillStamina();

        if (_lifeFolder.childCount < _player.CurrentHealth)
        {
            AddLife(1);
        }
        else if (_lifeFolder.childCount > _player.CurrentHealth)
        {
            RemoveLife(1);
        }

        if(_player.CurrentStamina == 4 && _staminaIconCount != 4)
        {
            AddStamina(1, true);
            _staminaContainer[3].SetActive(true);
        }
    }

    private void ApplyCooldown()
    {
        _chargeCooldownTimerForImage += Time.deltaTime;
        _chargeCooldownTimerForText -= Time.deltaTime;

        if (_chargeCooldownTimerForImage > _player.ChargeAttackCooldown)
        {
            _chargeCooldownTimerForImage = 0f;
            _chargeCooldownTimerForText = _player.ChargeAttackCooldown;
            _isChargeCooldown = false;
            _imageCooldown.fillAmount = 1f;
            _textCooldown.gameObject.SetActive(false);
        }
        else
        {
            float rounded = Mathf.Round(_chargeCooldownTimerForText * 10f) * 0.1f;
            _textCooldown.text = rounded.ToString();
            _imageCooldown.fillAmount = _chargeCooldownTimerForImage / _player.ChargeAttackCooldown;
        }
    }

    private void ResetGame(Dictionary<string, object> message)
    {
        bool needReset = (bool)message["reset"];

        _player = FindObjectOfType(typeof(Player)) as Player;

        if (needReset == false) return;

        _player = FindObjectOfType(typeof(Player)) as Player;

        AddLife(_player.MaxHealth);

        _staminaIconCount = 0;
        AddStamina(_player.MaxStamina, true);
        _staminaContainer[3].SetActive(false);

        _player.NbOfCoins = 0;
        _coinsText.text = _player.NbOfCoins.ToString("n0");
    }

    private void ResetSecret(Dictionary<string, object> message)
    {
        foreach (Transform t in _staminaFolder)
        {
            t.GetComponent<Image>().color = _originalColor;
        }

        _secretUI.GetComponent<TextMeshProUGUI>().color = _originalSecretUIColor;
        _secretUI.transform.parent.parent.gameObject.SetActive(false);
        _staminaFadeQty = 4;
        _player.SetUnlimitedStamina(false);
    }

    private void PauseStaminaSecret(Dictionary<string, object> message)
    {
        bool value = (bool)message["pause"];
        
        _pauseStaminaSecret = value;
    }
    
    public void UpdateCoins(Dictionary<string, object> message)
    {
        var amount = (int)message["coin"];

        if (amount == 0) return;

        _player.NbOfCoins += amount;
        _coinsText.text = _player.NbOfCoins.ToString("n0");
    }

    private void HealthChange(Dictionary<string, object> message)
    {
        if ((int)message["health"] > 0)
            AddLife((int)message["health"]);
        else
            RemoveLife(-(int)message["health"]);
    }

    public void AddLife(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (_lifeFolder.childCount < _player.MaxHealth)
            {
                Instantiate(_life, _lifeFolder);
                _player.CurrentHealth = _lifeFolder.childCount;
            }
        }
    }

    public void RemoveLife(int value)
    {
        for (int i = 0; i < value; i++)
        {
            if (_lifeFolder.childCount != 0)
            {
                var lastLife = _lifeFolder.GetChild(_lifeFolder.childCount - 1).gameObject;
                lastLife.transform.SetParent(_lifeFadingContainer.transform, false);
                StartCoroutine(FadeOutLife(lastLife));
            }
            else return;
        }
    }

    private IEnumerator FadeOutLife(GameObject lifeUI)
    {
        var duration = 1f;
        var timePassed = 0f;
        var lifeImage = lifeUI.GetComponent<Image>();

        var startcolor = lifeImage.color = Color.red;
        var targetColor = new Color(1, 0, 0, 0);

        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;

            lifeImage.color = Color.Lerp(startcolor, targetColor, timePassed / duration);

            yield return null;
        }

        Destroy(lifeUI);
    }
    
    

    private void StaminaChange(Dictionary<string, object> message)
    {
        if (_player.UnlimitedStamina || _player.GodMode)
            return;

        if (message.ContainsKey("stamina"))
        {
            if ((int)message["stamina"] > 0)
                AddStamina((int)message["stamina"], false);
            else if ((int)message["stamina"] < 0)
                RemoveStamina(-(int)message["stamina"]);
        }
        else
            FlashingStamina((int)message["staminaMissing"]);
    }

    private IEnumerator FadeOutStamina()
    {
        if(!_pauseStaminaSecret)
        {
            float elapsedTime = 0f;

            while (elapsedTime < _staminaFadeLength)
            {
                elapsedTime += Time.deltaTime;
                Color colorLerp = Color.Lerp(_originalColor, _targetColor, elapsedTime / _staminaFadeLength);

                foreach (Transform child in _staminaFolder)
                {
                    child.GetComponent<Image>().color = colorLerp;
                }

                yield return null;
            }

            if (_staminaFadeQty-- > 0)
                StartCoroutine(FadeInStamina());
            else
                _staminaFadeQty = 4;
        }
    }
    
    private IEnumerator FadeInStamina()
    {
        if (!_pauseStaminaSecret)
        {
            float elapsedTime = 0f;

            while (elapsedTime < _staminaFadeLength)
            {
                elapsedTime += Time.deltaTime;
                Color colorLerp = Color.Lerp(_targetColor, _originalColor, elapsedTime / _staminaFadeLength);

                foreach (Transform child in _staminaFolder)
                {
                    child.GetComponent<Image>().color = colorLerp;
                }

                yield return null;
            }

            if (_staminaFadeQty-- > 0)
                StartCoroutine(FadeOutStamina());
            else
                _staminaFadeQty = 4;
        }
    }

    public void TriggerLoopFadeStamina(bool startFadedIn = true)
    {
        if(startFadedIn)
            StartCoroutine(FadeOutStamina());
        else
            StartCoroutine(FadeInStamina());
    }

    private IEnumerator FadeOutSecretUI()
    {
        if (!_pauseStaminaSecret)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 3f)
            {
                elapsedTime += Time.deltaTime;
                Color colorLerp = Color.Lerp(_originalSecretUIColor, _targetSecretUIColor,
                    elapsedTime / _staminaFadeLength);

                _secretUI.GetComponent<TextMeshProUGUI>().color = colorLerp;

                yield return null;
            }
        }
    }

    public void TriggerFadeOutSecretUI()
    {
        StartCoroutine(FadeOutSecretUI());
    }


    public void AddStamina(int value, bool isReset)
    {
        if (isReset || value > 1)
        {
            for (int i = 0; i < value; i++)
            {
                var staminaImageObject = _staminaContainer[i];
                staminaImageObject.SetActive(true);
                staminaImageObject.GetComponent<Image>().fillAmount = 1f;

                _staminaIconCount++;

                if(_staminaImageQueue.Count != 0)
                {
                    for(int j = 0; i < _staminaImageQueue.Count; j++)
                    {
                        _staminaImageQueue.Dequeue();
                    }
                }
            }
            _player.CurrentStamina = _player.MaxStamina;
        }
        else
        {
            if (value == 1 && _player.CurrentStamina < _player.MaxStamina)
            {
                var staminaImageObject = _staminaContainer[_player.CurrentStamina];
                Image image = staminaImageObject.GetComponent<Image>();
                _staminaImageQueue.Enqueue(image);
            }
        }
    }


    public void RemoveStamina(int value)
    {
        for (int i = 0; i < value; i++)
        {
            var staminaImageObject = _staminaContainer[_player.CurrentStamina - i - 1];
            Image image = staminaImageObject.GetComponent<Image>();
            image.fillAmount = 0f;

            if(_staminaImageQueue.Count != 0)
                _staminaImageQueue.Dequeue().fillAmount = 0f;

            _staminaImageQueue.Enqueue(image);
        }
    }

    private void FillStamina()
    {
        if (_staminaImageQueue.Count == 0 || _player.UnlimitedStamina)
            return;

        if (_player.PercentOfStaminaRefilled == 0f)
        {
            Image image = _staminaImageQueue.Dequeue();
            image.fillAmount = 1f;
            image.color = _originalColor;
            var anim = image.gameObject.GetComponent<Animation>();
            anim.Play("Anim_StaminaScale");
            StartCoroutine(ScaleAnimationDone(anim, image));
        }
        else
        {
            Image image = _staminaImageQueue.Peek();
            image.fillAmount = _player.PercentOfStaminaRefilled;
        }
    }

    private void FlashingStamina(int amountMissing)
    {
        switch(amountMissing)
        {
            case 1:
                _staminaImageQueue.Peek().gameObject.GetComponent<Animation>().Play("Anim_StaminaColor");
                StartCoroutine(ColorAnimationDone(_staminaContainer));
                break;
            case 2:
                _staminaContainer[0].GetComponent<Animation>().Play("Anim_StaminaColor");
                if(_staminaContainer[1].GetComponent<Image>().fillAmount == 0f)
                    _staminaContainer[1].GetComponent<Image>().fillAmount = 1f;
                _staminaContainer[1].GetComponent<Animation>().Play("Anim_StaminaColor");
                StartCoroutine(ColorAnimationDone(_staminaContainer));
                break;
            default:
                break;
        }
    }

    public IEnumerator ColorAnimationDone(GameObject[] theObject)
    {
        var anim1 = theObject[0].GetComponent<Animation>();
        var anim2 = theObject[1].GetComponent<Animation>();

        while (anim1.IsPlaying("Anim_StaminaColor") ||
               anim2.IsPlaying("Anim_StaminaColor"))
        {
            yield return null;
        }
        if(theObject[1].GetComponent<Image>().fillAmount == 1f && _player.CurrentStamina != 2)
            theObject[1].GetComponent<Image>().fillAmount = 0f;
    }
    public IEnumerator ScaleAnimationDone(Animation anim, Image image)
    {
        while (anim.IsPlaying("Anim_StaminaScale") && image.fillAmount == 1f)
        {
            yield return null;
        }
        anim.Stop("Anim_StaminaScale");
        anim.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void StartCooldown(Dictionary<string, object> message)
    {
        if ((string)message["attack"] == "charge")
        {
            _isChargeCooldown = true;
            _imageCooldown.fillAmount = 0f;
            _textCooldown.gameObject.SetActive(true);
        }
    }

    private void ResetStaminaToDefault()
    {
        for (int i = 0; i < _staminaContainer.Length; i++)
        {
            _staminaContainer[i].GetComponent<Image>().color = _originalColor;
            _staminaContainer[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    private void DestroyFadingLifes()
    {
        foreach (Transform c in _lifeFadingContainer)
        {
            Destroy(c.gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.PLAYER_HEALTH_CHANGE, HealthChange);
        EventManager.StartListening(Events.PLAYER_STAMINA_CHANGE, StaminaChange);
        EventManager.StartListening(Events.RESET, ResetGame);
        EventManager.StartListening(Events.RESET, ResetSecret);
        EventManager.StartListening(Events.SWITCH_ROOM, ResetSecret);
        EventManager.StartListening(Events.PAUSE, PauseStaminaSecret);
        EventManager.StartListening(Events.PLAYER_COIN_CHANGE, UpdateCoins);
        EventManager.StartListening(Events.PLAYER_CHARGE_ATTACK, StartCooldown);
        _coinsText.text = _player.NbOfCoins.ToString("n0");
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PLAYER_HEALTH_CHANGE, HealthChange);
        EventManager.StopListening(Events.PLAYER_STAMINA_CHANGE, StaminaChange);
        EventManager.StopListening(Events.RESET, ResetGame);
        EventManager.StopListening(Events.RESET, ResetSecret);
        EventManager.StopListening(Events.SWITCH_ROOM, ResetSecret);
        EventManager.StopListening(Events.PAUSE, PauseStaminaSecret);
        EventManager.StopListening(Events.PLAYER_COIN_CHANGE, UpdateCoins);
        EventManager.StopListening(Events.PLAYER_CHARGE_ATTACK, StartCooldown);

        ResetStaminaToDefault();
        DestroyFadingLifes();
    }
}
