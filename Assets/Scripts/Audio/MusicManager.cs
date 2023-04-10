using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MusicLabel
{
    Peaceful,
    Combat,
    Menu,
    MusicLabelEnd
}

public class MusicManager : AudioManager
{
    private List<List<Music>> _labelMusics = new();
    private List<List<int>> _labelHasPlayed = new();
    
    private bool _hostilePlaying;
    public int HostileNumber { get; set; }
    
    private Music _mainTrack;

    [SerializeField] private float _fadeDuration = 1f;

    private void Start()
    {
        float fl = PlayerPrefs.GetFloat("MusicVolume", 0);
        AdjustVolume("MusicVolume", fl);
    }
    
    void Awake()
    {
        for (int i = 0; i < (int)MusicLabel.MusicLabelEnd; i++)
        {
            _labelMusics.Add(new());
            _labelHasPlayed.Add(new());
        }
    }
    
    public void ResetManager(AudioSource[] audioSources)
    {
        List<List<Music>> newLabelMusics = new List<List<Music>>();
        
        foreach (List<Music> musics in _labelMusics)
        {
            List<Music> newMusics = new List<Music>();
            
            foreach (Music music in musics)
            {
                if (music._persistent)
                {
                    newMusics.Add(music);
                }
                else
                {
                    Destroy(music._source);
                }
            }
            
            newLabelMusics.Add(newMusics);
        }

        _labelMusics = newLabelMusics;

        _mainTrack = null;

        _hostilePlaying = false;
        HostileNumber = 0;

        StopAllCoroutines();
    }
    
    public void Play(Music music)
    {
        if (music == null)
            return;
        music._source.Play();
    }
    
    private void Stop()
    {
        if (_mainTrack != null)
        {
            if (_mainTrack._source.isPlaying)
            {
                _mainTrack._source.Stop();
            }
        }
    }

    private void Pause()
    {
        if (_mainTrack != null)
        {
            if (_mainTrack._source.isPlaying)
            {
                _mainTrack._source.Pause();
            }
        }
    }

    public void AddMusic(AudioClip a, MusicAddressables music, MusicLabel mlabel)
    {
        Music newMusic = new()
        {
            _name = a.ToString(),
            _clip = a,
            _volume = music._volume,
            _persistent = music._persistent,
            _source = gameObject.AddComponent<AudioSource>()
        };
        newMusic._source.clip = a;
        newMusic._source.volume = newMusic._volume;
        newMusic._source.pitch = 1f;
        newMusic._source.playOnAwake = false;

        newMusic._source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Music")[0];
        
        _labelMusics[(int)mlabel].Add(newMusic);
    }

    private void PlayMusic(MusicLabel label, bool fade = false)
    {
        int intLabel = (int)label;

        int index = _labelMusics[intLabel].IndexOf(_mainTrack);
        if(index!=-1)
            _labelHasPlayed[intLabel].Add(index);
        
        if (_labelMusics[intLabel].Count == _labelHasPlayed[intLabel].Count)
            _labelHasPlayed[intLabel] = new();
        
        List<int> idx = Enumerable.Range(0, _labelMusics[intLabel].Count).ToList();
        idx = idx.Except(_labelHasPlayed[intLabel]).ToList();
        
        _mainTrack = _labelMusics[intLabel][idx[Random.Range(0, idx.Count)]];

        if(fade)
        {
            _mainTrack._source.volume = 0f;
            StartCoroutine(StartFade(_mainTrack._source, 1f, _mainTrack._volume));
        }
            
        Play(_mainTrack);
    }

    public void FullyLoaded(MusicLabel mlabel)
    {
        int intlabel = (int)mlabel;
        if(mlabel == MusicLabel.Peaceful || mlabel == MusicLabel.Menu)
        {
            if (_labelMusics[intlabel].Count != 0)
            {
                Stop();
                _mainTrack = new();
                _mainTrack = _labelMusics[intlabel][Random.Range(0, _labelMusics[intlabel].Count)];
                Play(_mainTrack);
            }
        }
    }
    
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            if(audioSource.volume == 0)
                audioSource.Stop();
            yield return null;
        }
        yield break;
    }

    private void Update()
    {
        if (_mainTrack == null) return;
        
        if (HostileNumber > 0 && !_hostilePlaying)
        {
            StartCoroutine(StartFade(_mainTrack._source, _fadeDuration, 0f));
            _mainTrack = new();
            int index = _labelMusics[(int)MusicLabel.Combat].IndexOf(_mainTrack);
            _labelHasPlayed[(int)MusicLabel.Combat].Add(index);
            _hostilePlaying = true;
            PlayMusic(MusicLabel.Combat, true);
        }
        else if (HostileNumber > 0 && _hostilePlaying)
        {
            if (!_mainTrack._source.isPlaying)
            {
                int index = _labelMusics[(int)MusicLabel.Combat].IndexOf(_mainTrack);
                _labelHasPlayed[(int)MusicLabel.Combat].Add(index);
                PlayMusic(MusicLabel.Combat);
            }
        }
        else
        {
            if (!_mainTrack._source.isPlaying && !_hostilePlaying && _labelMusics[(int)MusicLabel.Peaceful].Count != 0)
            {
                int index = _labelMusics[(int)MusicLabel.Peaceful].IndexOf(_mainTrack);
                _labelHasPlayed[(int)MusicLabel.Peaceful].Add(index);
                PlayMusic(MusicLabel.Peaceful);
            }
            else if(_hostilePlaying)
            {
                _hostilePlaying = false;
                StartCoroutine(StartFade(_mainTrack._source, _fadeDuration, 0f));
                PlayMusic(MusicLabel.Peaceful, true);
            }
            else if(!_mainTrack._source.isPlaying)
            {
                PlayMusic(MusicLabel.Menu);
            }
        }
    }   
}
