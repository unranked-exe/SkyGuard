using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //Called when the player clicks the "Play" button.
    public void PlayGame()
    {
        //Loads the game scene into the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //Called when the player clicks the "Quit" button.
    public void OnApplicationQuit()
    {
        //Quits the application.
        Application.Quit();
    }
}
