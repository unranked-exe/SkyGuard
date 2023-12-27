using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Reference to the Game Over Screen
    [SerializeField] private GameObject gameOverScreen;
    //Reference to the Pause Screen
    [SerializeField] private GameObject pauseScreen;


    //Reference to the score text component
    [SerializeField] private TextMeshProUGUI scoreText;
    //Reference to the score counter in UI.
    [SerializeField] private TextMeshProUGUI scoreCounterInEnd;

    //Reference to the selected plane text component in UI Window
    [SerializeField] private TextMeshProUGUI selectedPlaneUIWindow;


    private void Awake()
    {
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
            case GameState.Paused:
                //Displays the Pause Screen.
                pauseScreen.SetActive(true);
                break;
            case GameState.GameOver:
                StartCoroutine(HandleGameOver());
                IEnumerator HandleGameOver()
                {
                    //Sets the collision effect to active.
                    GameManager.instance.collisionEffect.SetActive(true);
                    //Sets the score counter in the Game Over Screen to the score.
                    scoreCounterInEnd.text = "Score: " + GameManager.instance.GetScore();
                    //Waits for 2 seconds (unscaled time) to play crash animation.
                    yield return new WaitForSecondsRealtime(2f);
                    //Displays the Game Over Screen.
                    gameOverScreen.SetActive(true);
                }
                break;
            case GameState.Restart:
                gameOverScreen.SetActive(false);
                break;
            case GameState.Resume:
                //Deactivates the Pause Screen.
                pauseScreen.SetActive(false);
                GameManager.instance.UpdateGameState(GameState.Playing);
                break;
            default:
                break;
        }
    }

    //Called when the Resume button is pressed.
    public void Resume()
    {
        //Sets the GameState to Resume state.
        GameManager.instance.UpdateGameState(GameState.Resume);
    }
    
    //Called when the Restart button is pressed.
    public void Restart()
    {
        //Sets the GameState to Restart.
        GameManager.instance.UpdateGameState(GameState.Restart);
    }

    //Called when the Exit button is pressed on both the Game Over Screen and the Pause Screen.
    public void ExitToMenu()
    {
        //Sets the GameState to ExitToMenu.
        GameManager.instance.UpdateGameState(GameState.ExitToMenu);
    }

    public void BackToWindow()
    {
        if (GameManager.instance.State == GameState.Paused)
        {
            //Activates the Pause Screen.
            pauseScreen.SetActive(true);
        }
        else if (GameManager.instance.State == GameState.GameOver)
        {
            //Activates the Game Over Screen.
            gameOverScreen.SetActive(true);
        }
    }

    //Called by Game Manager to update the score text.
    public void UpdateScore(int scoreToDisplay)
    {
        //Sets the score text to the score counter.
        scoreText.text = scoreToDisplay.ToString();
    }

    public void UpdateSelectedPlane(string planeName)
    {
        //Sets the selected plane text to the name of the plane.
        selectedPlaneUIWindow.text = "Plane Name: " + planeName;
    }

    //Called when the user enters bearing in input field.
    public void readBearing(string input)
    {
        //Checks if the input is 3 characters long just like a bearing.
        //Also checks if a plane is selected.
        if (input.Length == 3 && GameManager.instance.PreviousSelection != null)
        {
            //Converts the input to an integer.
            int bearing = int.Parse(input);
            //Checks if the bearing is between 0 and 360.
            if (bearing <= 360)
            {
                //Calls the MoveRotation method in the Game Manager.
                GameManager.instance.MoveRotation(bearing);
            }
        }
    }

    //Audio Functions
    public void ChangeBGMusicVolume(float volume)
    {
        AudioManager.instance.ChangeBGMusicVolume(volume);
    }

    public void ToggleBGMusic(bool toggle)
    {
        AudioManager.instance.ToggleBGMusic(toggle);
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioManager.instance.ChangeSFXVolume(volume);
    }

    private void Update()
    {
        //Checks if the user presses the escape key.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Checks if the game is paused.
            if (GameManager.instance.State == GameState.Paused)
            {
                //Resumes the game.
                GameManager.instance.UpdateGameState(GameState.Resume);
            }
            else if (GameManager.instance.State == GameState.GameOver)
            {
                //Exits to the main menu.
                GameManager.instance.UpdateGameState(GameState.ExitToMenu);
            }
            else
            {
                //Pauses the game.
                GameManager.instance.UpdateGameState(GameState.Paused);
            }
        }
    }
}
