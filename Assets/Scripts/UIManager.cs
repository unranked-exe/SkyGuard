using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Reference to the GameOverScreen
    [SerializeField] private GameObject gameOverScreen;

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
            case GameState.GameOver:
                StartCoroutine(HandleGameOver());
                IEnumerator HandleGameOver()
                {
                    //Sets the collision effect to active.
                    GameManager.instance.collisionEffect.SetActive(true);
                    //Sets the score counter in the GameOverScreen to the score.
                    scoreCounterInEnd.text = "Score: " + GameManager.instance.GetScore();
                    //Waits for 2 seconds (unscaled time) to play crash animation.
                    yield return new WaitForSecondsRealtime(2f);
                    //Displays the GameOverScreen.
                    gameOverScreen.SetActive(true);
                }
                break;
            case GameState.Playing:
                //Displays the GameOverScreen..
                gameOverScreen.SetActive(false);
                break;
            default:
                break;
        }
    }

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

    //Called when the Exit button is pressed on both the GameOverScreen and the PauseScreen.
    public void ExitToMenu()
    {
        //Sets the GameState to ExitToMenu.
        GameManager.instance.UpdateGameState(GameState.ExitToMenu);
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
            //Calls the MoveRotation method in the Game Manager.
            GameManager.instance.MoveRotation(bearing);
        }
    }
}
