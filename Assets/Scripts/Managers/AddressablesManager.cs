using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressablesManager : MonoBehaviour
{
    public enum PrefabLabel
    {
        Audio,
        PrefabLabelEnd
    }
    

    [SerializeField] private List<MusicLabelAddressables> _musicLabels;
    [SerializeField] private List<SpeechLabelAddressables> _speechLabels;
    [SerializeField] private List<SpeechAddressables> _speechSolos;
    [SerializeField] private List<AmbientLabelAddressables> _ambientLabels;
    [SerializeField] private List<AmbientAddressables> _ambientSolos;
    [SerializeField] private List<FXLabelAddressables> _fxLabels;
    [SerializeField] private List<FXAddressables> _fxSolos;
    [SerializeField] private List<PrefabLabel> _prefabLabels;
    
    private void Awake()
    {
        Addressables.InitializeAsync();
        
        foreach (MusicLabelAddressables mLabelA in _musicLabels)
        {
            AddressablesMusicLabel(mLabelA);
        }
        
        foreach (SpeechLabelAddressables sLabelA in _speechLabels)
        {
            AddressablesSpeechLabel(sLabelA);
        }
        
        foreach (SpeechAddressables speech in _speechSolos)
        {
            AddressablesSpeechSolo(speech);
        }

        foreach (FXLabelAddressables fxLabelA in _fxLabels)
        {
            AddressablesFXLabel(fxLabelA);
        }

        foreach (FXAddressables fx in _fxSolos)
        {
            AddressablesFXSolo(fx);
        }
        
        foreach (AmbientLabelAddressables aLabelA in _ambientLabels)
        {
            AddressablesAmbientLabel(aLabelA);
        }

        foreach (AmbientAddressables ambient in _ambientSolos)
        {
            AddressablesAmbientSolo(ambient);
        }
        
        foreach (PrefabLabel pLabel in _prefabLabels)
        {
            AddressablesPrefabLabel(pLabel);
        }
    }
    
    //AUDIO MUSIC
    
    public void AddressablesMusicLabel(MusicLabelAddressables mLabelA)
    {
        Addressables.LoadAssetsAsync<AudioClip>(mLabelA._label.ToString(), LoadCallback).Completed += handle => OnMusicLabelLoaded(handle, mLabelA);
    }
    
    void OnMusicLabelLoaded(AsyncOperationHandle<IList<AudioClip>> handle, MusicLabelAddressables mlabelA)
    {
        for (int i = 0; i < handle.Result.Count; i++)
        {
            AudioClip clip = handle.Result[i];
            MusicAddressables music = new()
            {
                _volume = mlabelA._volume,
                _persistent = mlabelA._persistent
            };
            FindObjectOfType<MusicManager>().AddMusic(clip, music, mlabelA._label);
        }

        FindObjectOfType<MusicManager>().FullyLoaded(mlabelA._label);
    }

    private void LoadCallback(AudioClip a) { }

    
    //AUDIO AMBIENT
    
    public void AddressablesAmbientLabel(AmbientLabelAddressables aLabelA)
    {
        Addressables.LoadAssetsAsync<AudioClip>(aLabelA._label.ToString(), LoadCallbackAmbient).Completed += handle => OnAmbientLabelLoaded(handle, aLabelA);
    }
    
    void OnAmbientLabelLoaded(AsyncOperationHandle<IList<AudioClip>> handle, AmbientLabelAddressables aLabelA)
    {
        for (int i = 0; i < handle.Result.Count; i++)
        {
            AudioClip clip = handle.Result[i];
            AmbientAddressables ambient = new()
            {
                _volume = aLabelA._volume,
                _pitch = 1f,
                _loop = aLabelA._loop,
                _playOnAwake = aLabelA._playOnAwake,
                _spatialBlend = aLabelA._spatialBlend,
                _minDistance = aLabelA._minDistance,
                _maxDistance = aLabelA._maxDistance,
                _audioRolloff = aLabelA._audioRolloff
            };
            FindObjectOfType<AmbientManager>().AddAmbient(clip, ambient, aLabelA._label, gameObject);
        }
    }

    public void AddressablesAmbientSolo(AmbientAddressables ambient)
    {
        Addressables.LoadAssetAsync<AudioClip>(ambient._path).Completed += handle => OnAmbientSoloLoaded(handle, ambient);
    }
    
    void OnAmbientSoloLoaded(AsyncOperationHandle<AudioClip> handle, AmbientAddressables ambient)
    {
        AudioClip clip = handle.Result;
        FindObjectOfType<AmbientManager>().AddAmbient(clip, ambient, AmbientLabel.AmbientLabelEnd, gameObject);
    }

    private void LoadCallbackAmbient(AudioClip a) { }

    
    //AUDIO SPEECH
    
    public void AddressablesSpeechSolo(SpeechAddressables speech)
    {
        Addressables.LoadAssetAsync<AudioClip>(speech._path).Completed += handle => OnSpeechSoloLoaded(handle, speech);
    }
    
    void OnSpeechSoloLoaded(AsyncOperationHandle<AudioClip> handle, SpeechAddressables speech)
    {
        AudioClip clip = handle.Result;
        FindObjectOfType<SpeechManager>().AddSpeech(clip, speech, SpeechLabel.SpeechLabelEnd, gameObject);
    }

    public void AddressablesSpeechLabel(SpeechLabelAddressables sLabelA)
    {
        Addressables.LoadAssetsAsync<AudioClip>(sLabelA._label.ToString(), LoadCallbackSpeech).Completed += handle => OnSpeechLabelLoaded(handle, sLabelA);
    }
    
    void OnSpeechLabelLoaded(AsyncOperationHandle<IList<AudioClip>> handle, SpeechLabelAddressables sLabelA)
    {
        for (int i = 0; i < handle.Result.Count; i++)
        {
            AudioClip clip = handle.Result[i];
            SpeechAddressables speech = new();
            speech._volume = sLabelA._volume;
            speech._pitchMin = 1f;
            speech._pitchMax = 1f;
            FindObjectOfType<SpeechManager>().AddSpeech(clip, speech, sLabelA._label);
        }
    }
    
    private void LoadCallbackSpeech(AudioClip a) { }


    //AUDIO FX
    
    public void AddressablesFXLabel(FXLabelAddressables fxLabelA)
    {
        Addressables.LoadAssetsAsync<AudioClip>(fxLabelA._label.ToString(), LoadCallbackFX).Completed += handle => OnFXLoaded(handle, fxLabelA);
    }
    
    void OnFXLoaded(AsyncOperationHandle<IList<AudioClip>> handle, FXLabelAddressables fxLabelA)
    {
        for (int i = 0; i < handle.Result.Count; i++)
        {
            AudioClip clip = handle.Result[i];
            FXAddressables fx = new();
            fx._volume = fxLabelA._volume;
            fx._pitchMin = 1f;
            fx._pitchMax = 1f;
            FindObjectOfType<FXManager>().AddFX(clip, fx, fxLabelA._label, gameObject);
        }
    }

    private void LoadCallbackFX(AudioClip a) { }
  
            //SOUND SOLOS
            
    public void AddressablesFXSolo(FXAddressables fx)
    {
        Addressables.LoadAssetAsync<AudioClip>(fx._path).Completed += handle => OnFXSoloLoaded(handle, fx);
    }
    
    void OnFXSoloLoaded(AsyncOperationHandle<AudioClip> handle, FXAddressables fx)
    {
        AudioClip clip = handle.Result;
        FindObjectOfType<FXManager>().AddFX(clip, fx, FXLabel.FXLabelEnd, gameObject);
    }


    //PREFABS
    
    public void AddressablesPrefabLabel(PrefabLabel pLabel)
    {
        Addressables.LoadAssetsAsync<GameObject>(pLabel, PrefabLoadCallback).Completed += handle => OnPrefabLabelLoaded(handle, pLabel);
    }
    
    void OnPrefabLabelLoaded(AsyncOperationHandle<IList<GameObject>> handle, PrefabLabel pLabel)
    {
        foreach (GameObject go in handle.Result)
        {
            //DO SOMETHING WITH PREFAB
            switch (pLabel)
            {
                case PrefabLabel.Audio:
                    
                    break;
            }
        }
        //Debug.Log(handle.Result);
    }

    private void PrefabLoadCallback(GameObject a) { }
    
    //SPRITES
    
    public void AddressablesSprite()
    {
        Debug.Log("Load Addressables Sprite");
        Addressables.LoadAssetAsync<Sprite>("Sprite").Completed += handle => OnSpriteLoaded(handle);
    }

    void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
    {
        //image.sprite = handle.Result;
    }
}