using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Creates 1 instance of the AudioManager
    private static AudioManager instance;

    //Reference to the audio sources
    [SerializeField] private AudioSource backgroundMusicSource;

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
    }
}
