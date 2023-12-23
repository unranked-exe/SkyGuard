using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Creates 1 instance of the AudioManager
    public static AudioManager instance;

    [Header("-------- Audio Sources --------")]
    //Reference to the audio sources
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("-------- Audio Clips --------")]
    //Reference to the audio clips
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip spawnSound;

    private void Awake()
    {
        //If there is no instance of the AudioManager, set it to this
        if (instance == null)
        {
            instance = this;
            //Dont destroy this instance of the AudioManager when loading a new scene
            DontDestroyOnLoad(gameObject);
            //Sets the background music clip and plays it
            backgroundMusicSource.clip = backgroundSound;
            backgroundMusicSource.Play();
        }
        //If there is an instance of the AudioManager, destroy this
        else
        {
            Destroy(gameObject);
        }
        //Subscribes to the OnGameStateChanged event.
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }
    private void OnDestroy()
    {
        //Unsubscribes from the OnGameStateChanged event.
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    //Function that is called when the OnGameStateChanged event is invoked.
    private void GameManager_OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.GameOver:
                //Plays the crash sound effect.
                soundEffectSource.PlayOneShot(crashSound);
                break;
        }
    }

    //Function to play the spawn sound effect.
    public void PlaySpawnSound()
    {
        soundEffectSource.PlayOneShot(spawnSound);
    }

    public void ChangeBGMusicVolume(float volume)
    {
        //Sets the background music volume to the volume passed in.
        backgroundMusicSource.volume = volume;
    }

    public void ToggleBGMusic(bool toggle)
    {
        //Inverts the toggle passed in.
        toggle = !toggle;
        //Sets the background music to the toggle passed in.
        backgroundMusicSource.mute = toggle;
    }

    public void ChangeSFXVolume(float volume)
    {
        //Sets the sound effect volume to the volume passed in.
        soundEffectSource.volume = volume;
    }
}
