using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Creates 1 instance of the AudioManager
    private static AudioManager instance;

    [Header("-------- Audio Sources --------")]
    //Reference to the audio sources
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundEffectSource;

    [Header("-------- Audio Clips --------")]
    //Reference to the audio clips
    [SerializeField] private AudioClip backgroundSound;
    [SerializeField] private AudioClip crashSound;

    private void Awake()
    {
        //If there is no instance of the AudioManager, set it to this
        if (instance == null)
        {
            instance = this;
            //Dont destroy this instance of the AudioManager when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
        //If there is an instance of the AudioManager, destroy this
        else
        {
            Destroy(gameObject);
        }
        //Subscribes to the OnGameStateChanged event.
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

        backgroundMusicSource.clip = backgroundSound;
        backgroundMusicSource.Play();
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
}
