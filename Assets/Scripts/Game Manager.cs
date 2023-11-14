using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Reference to the instance of the GameManager
    public static GameManager instance;

    //Variable to the current state of the game.
    public GameState State;

    private void Awake()
    {
        //Sets the instance of the GameManager to this.
        instance = this;
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
            case GameState.Paused:
                //Do something
                break;
            case GameState.GameOver:
                //Do something
                break;
            default:
                //For debugging purposes in later stages of development.
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }


    //Enumertor that holds the different states of the game.
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
}
