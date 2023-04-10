using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSoundController : MonoBehaviour
{
    private SpeechManager _speechManager;
    private FXManager _fxManager;

    private void Start()
    {
        _speechManager = FindObjectOfType<SpeechManager>();
        _fxManager = FindObjectOfType<FXManager>();
    }

    private void FangSound()
    {
        _fxManager.PlaySound("fangs", gameObject, false, 0.2f);
    }

    private void SlashSound()
    {
        _fxManager.PlaySound("slash", gameObject, false, 0.2f);
    }

    private void BiteSound()
    {
        if(Random.Range(0,20) == 0)
            _speechManager.PlaySpeech("malfunction" + Random.Range(0,6));
        else
            _speechManager.PlaySpeech("bite");
    }

    private void ClawSound()
    {
        if(Random.Range(0,20) == 0)
            _speechManager.PlaySpeech("malfunction" + Random.Range(0,6));
        else
            _speechManager.PlaySpeech("claw");
    }

    private void FootstepsSound()
    {
        _fxManager.PlaySound("footsteps" + Random.Range(1, 4), gameObject);
    }

    private void FootstepsFrontSound()
    {
        _fxManager.PlaySound("footstepsfront" + Random.Range(1, 2), gameObject);
    }

    private void FootstepsRearSound()
    {
        _fxManager.PlaySound("footstepsrear" + Random.Range(1, 2), gameObject);
    }

    private void PantingSound(Dictionary<string, object> message)
    {
        int stamina = GetComponent<Player>().CurrentStamina;
        
        if (stamina == 2)
        {
            _fxManager.StopSound("fox_panting", gameObject);
            _fxManager.PlaySound("breath_in", gameObject);
        }
        
        if (stamina < 2)
            _fxManager.PlaySound("fox_panting", gameObject);
    }
    
    private void ChargeAttackSound()
    {
        _fxManager.PlaySound("charge_attack", gameObject);
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.PLAYER_STAMINA_CHANGE, PantingSound);
    }
    
    private void OnDisable()
    {
        EventManager.StopListening(Events.PLAYER_STAMINA_CHANGE, PantingSound);
    }
}
