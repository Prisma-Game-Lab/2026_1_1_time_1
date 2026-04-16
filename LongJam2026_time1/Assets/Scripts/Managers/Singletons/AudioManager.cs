using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, loopingSfxSource;
    
    public AudioMixerGroup sfxMixerGroup;

    [SerializeField] private string menuMusic;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        PlayMusic(menuMusic);

    }
   
    private string currentlyToggledSong = null;
    private string originalSong = null;

    public void ToggleMusic(string newSong)
    {
    
    if (musicSource.clip == null || !musicSource.isPlaying)
    {
        PlayMusic(newSong);
        currentlyToggledSong = newSong;
        return;
    }

    if (musicSource.clip.name == newSong)
    {
        if (!string.IsNullOrEmpty(originalSong))
        {
            PlayMusic(originalSong);
            currentlyToggledSong = null;
        }
        else
        {
            musicSource.Stop();
            currentlyToggledSong = null;
        }
    }
    else
    {
        originalSong = musicSource.clip.name;
        PlayMusic(newSong);
        currentlyToggledSong = newSong;
    }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopLoopingSFX()
    {
        loopingSfxSource.Stop();
    }
    public void PlayMusic(string name)
    {
        AudioClip s = Array.Find(musicSounds, x => x.name == name);
        if (s != null)
        {
            musicSource.clip = s;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip s = Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            sfxSource.PlayOneShot(s);
        }
    }

    public void PlayLoopingSFX(string name)
    {
        AudioClip s = Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            loopingSfxSource.PlayOneShot(s);
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}