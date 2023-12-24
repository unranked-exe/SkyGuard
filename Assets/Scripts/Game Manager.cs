using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Reference to the instance of the GameManager
    public static GameManager instance;

    //Variable to hold the current state of the game.
    public GameState State;

    //Event that is called when the state of the game changes.
    public static event Action<GameState> OnGameStateChanged;

    //Reference to the collision effect object.
    public GameObject collisionEffect;

    //Variable to hold the previously selected plane.
    public GameObject PreviousSelection;

    //Reference to the UI Canvas.
    [SerializeField] private UIManager UIManager;

    //Reference to the score counter.
    private int score = 0;

    public float turning = 1f;
    
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
    }

    private void Start()
    {
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
                HandlePlaying();
                break;
            case GameState.EndOfRound:
                HandleEndOfRound();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.Resume:
                HandleResume();
                break;
            case GameState.GameOver:
                //Calls the HandleGameOver method that allows the manager to halt the timescale.
                HandleGameOver();
                break;
            case GameState.Restart:
                HandleRestart();
                break;
            case GameState.ExitToMenu:
                HandleExitToMenu();
                break;
            default:
                //For debugging purposes in later stages of development.
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        Debug.Log("State: " + State);
        //Calls the OnGameStateChanged event.
        OnGameStateChanged?.Invoke(State);
    }

    private void HandlePlaying()
    {
        //Calls a function that calls a private coroutine starts the spawning of planes.
        SpawnerScript.instance.StartSpawning();
    }
    private void HandleEndOfRound()
    {
        //Starts the coroutine that will delay the start of the next round.
        StartCoroutine(IntervalBetweenRound());
        //Checks if the game state now set to playing.
        if (State == GameState.Playing)
            //Stops the time delay coroutine.
            StopCoroutine(IntervalBetweenRound());
    }

    private void HandlePaused()
    {
        //Pauses all movement in the game.
        Time.timeScale = 0;
    }

    private void HandleResume()
    {
        //Resumes all movement in the game.
        Time.timeScale = 1;
    }
    
    private void HandleGameOver()
    {
        if (PreviousSelection != null)
            //Calls the PlaneDeselection function to clear Previous slection variable.
            PlaneDeselection();
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

    private void HandleExitToMenu()
    {
        //Resumes all movement in the game.
        Time.timeScale = 1;
        //Loads the main menu scene.
        SceneManager.LoadScene(0);
    }

    IEnumerator IntervalBetweenRound()
    {
        //A 25 second delay between the end of each round.
        //It is only this long as this delay is started from when last plane is spawned.
        yield return new WaitForSeconds(25);
        //Changes the state of the game back to playing.
        UpdateGameState(GameState.Playing);
    }

    //Function to add to the score counter.
    public void AddScore()
    {
        //Adds 1 to the score counter.
        score++;
        //Updates the score counter in the UI.
        UIManager.UpdateScore(score);
    }

    public int GetScore()
    {
        return score;
    }

    public void PlaneDeselection()
    {
        //Updates the selected plane text in the UI.
        UIManager.UpdateSelectedPlane("None");
        PreviousSelection.GetComponent<Renderer>().material.color = Color.white;
        PreviousSelection = null;
    }
    //Function to update the selected plane text.
    public void PlaneSelection()
    {
        //Updates the selected plane text in the UI.
        UIManager.UpdateSelectedPlane(PreviousSelection.name);
        PreviousSelection.GetComponent<Renderer>().material.color = Color.red;

    }

    public void MoveRotation(int bearing)
    {
        //Gets the Rigidbody2D component of the previously selected plane.
        Rigidbody2D rb = PreviousSelection.GetComponent<Rigidbody2D>();
        //Converts the bearing to the correct rotation.
        bearing = 360 - bearing;
        //Gets the current rotation of the plane.
        float currentBearing = rb.rotation;
        //Debug.Log("Current Bearing: " + currentBearing);
        float deltaBearing = bearing - currentBearing;
        //Checks if the bearing to change by is not 0.
        if (deltaBearing != 0)
        {
            //Debug.Log("Delta Bearing: " + deltaBearing);
            PlaneMoveScript planeMoveScript = PreviousSelection.GetComponent<PlaneMoveScript>();
            planeMoveScript.StartRotate(bearing);
        }
    }
}

//Enumertor that holds the different states of the game.
public enum GameState
{
    Playing,
    EndOfRound,
    Paused,
    Resume,
    GameOver,
    Restart,
    ExitToMenu
}
