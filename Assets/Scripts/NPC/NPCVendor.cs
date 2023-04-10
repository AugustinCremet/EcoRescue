using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCVendor : MonoBehaviour
{
    protected Player _player;
    protected UIManager _uiManager;
    protected Animator _animator;
    private bool _canSpeak = false;
    private bool _isSpeaking = false;
    public bool IsSpeaking { set { _isSpeaking = value; } }
    [SerializeField] protected GameObject _chatBubble;
    public GameObject ChatBubble { get { return _chatBubble; } }
    

    private void Start()
    {
        _uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;
        _animator = GetComponent<Animator>();
        EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", true } });
    }

    public void OnTriggerEnter(Collider other)
    {
        var hit = other.GetComponent<Player>();

        if (hit != null && !_isSpeaking)
        {
            // Show little talk bubble here
            _chatBubble.SetActive(true);
            _player = hit;
            _canSpeak = true;
            FindObjectOfType<SpeechManager>().PlaySpeech("pressetotalktovendor");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var hit = other.GetComponent<Player>();

        if (hit != null)
        {
            // Remove little talk bubble here
            _chatBubble.SetActive(false);
            _canSpeak = false;
        }
    }

    private void Update()
    {
        if (_canSpeak && _player.HasInteracted && !_isSpeaking)
        {
            _isSpeaking = true;
            _animator.SetTrigger("talking");
            EventManager.TriggerEvent(Events.PAUSE, new Dictionary<string, object> { { "pause", false } });
        }
    }

    public virtual void StartInteraction() {}
}
