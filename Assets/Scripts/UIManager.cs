using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Reference to the GameOverScreen
    [SerializeField] private GameObject gameOverScreen;

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

    //Called when the Restart button is pressed.
    public void Restart()
    {
        //Sets the GameState to Restart.
        GameManager.instance.UpdateGameState(GameState.Restart);
    } 
}
