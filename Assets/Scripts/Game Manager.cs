using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Reference to the instance of the GameManager
    public static GameManager instance;

    //Variable to the current state of the game.
    public GameState State;

    //Event that is called when the state of the game changes.
    public static event Action<GameState> OnGameStateChanged;

    //Reference to the collision effect object.
    public GameObject collisionEffect;
    
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //Sets the instance of the GameManager to this.
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this as there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets the current GameState to Playing.
        UpdateGameState(GameState.Playing);
    }

    public void UpdateGameState(GameState newState)
    {
        //Sets the new state of game to current state.
        State = newState;

        //Switch statement that checks the new state of the game.
        switch(newState)
        { 
            case GameState.Playing:
                //Do something
                break;
            case GameState.EndOfRound:
                //Do something
                break;
            case GameState.Paused:
                //Do something
                break;
            case GameState.GameOver:
                //Calls the HandleGameOver method that allowsy the manager to halt the timescale.
                HandleGameOver();
                break;
            case GameState.Restart:
                //Do something
                HandleRestart();
                break;
            default:
                //For debugging purposes in later stages of development.
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        //Calls the OnGameStateChanged event.
        OnGameStateChanged?.Invoke(State);
    }

    private void HandleGameOver()
    {
        //Pauses all movement in the game.
        Time.timeScale = 0;
    }

    private void HandleRestart()
    {
        //Resumes all movement in the game.
        Time.timeScale = 1;
        //Reloads the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

//Enumertor that holds the different states of the game.
public enum GameState
{
    Playing,
    EndOfRound,
    Paused,
    GameOver,
    Restart
}
