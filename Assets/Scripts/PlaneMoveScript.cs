using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneMoveScript : MonoBehaviour
{
    //Reference to the Rigidbody2D component of the plane
    [SerializeField] private Rigidbody2D _PlaneRB;
    //Reference to the speed of the plane
    [SerializeField] private float _PlaneSpeed;

    //Reference to the floating text game object
    [SerializeField] private GameObject floatingText;
    //Reference to the text mesh component of the floating text
    [SerializeField] private TextMeshPro textmesh;

    private void Awake()
    {
        
    }

    private void Start()
    {
        //Call the OutputBearing function
        OutputBearing();
        //Call the MovePlane function
        MovePlane();
    }

    //Function to move the plane upright
    private void MovePlane()
    {
        //Set the velocity of the plane to the up direction multiplied by the speed
        _PlaneRB.velocity = transform.up * _PlaneSpeed;
    }

    //Function to move the plane in the direction of the target bearing
    public void StartRotate(float bearing)
    {
        //Starts the coroutine to rotate the plane towards the target bearing.
        StartCoroutine(RotateUpdate(bearing));
    }
    
    //Function to rotate the plane towards the target bearing.
    IEnumerator RotateUpdate(float targetBearing)
    {
        //Get the current bearing of the plane.
        float currentBearing = _PlaneRB.rotation;
        //Runs untill the current bearing is the same as the target bearing.
        while (currentBearing != targetBearing)
        {
            MovePlane();
            //Calucaltes rotation per frame to smoothly turn towards the target bearing.
            currentBearing = Mathf.MoveTowardsAngle(currentBearing, targetBearing, GameManager.instance.turning);
            //Sets the rotation of the plane to the current bearing.
            _PlaneRB.MoveRotation(currentBearing);
            //Displays this change in bearing in the floating text.
            OutputBearing();
            //Wait for the next fixed update.
            yield return new WaitForFixedUpdate();
        }
        //Displays the final bearing in the floating text.
        OutputBearing();
    }

    //Function to output the bearing of the plane for Floating Text
    private void OutputBearing()
    {
        //Set the rotation of the Floating Text to upright
        floatingText.transform.rotation = Quaternion.identity;
        //Set the text of the floating text to the name of the plane
        textmesh.text = gameObject.name;
        //Get the rotation of the plane
        int rot = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
        //Calcualtes the real bearing of the plane
        rot = 360 - rot;
        //Converts the bearing to a string.
        string rotString = rot.ToString();
        //Checks if the bearing is less than 100 and adds 0's to the front 
        //of the string to make it 3 digits long.
        if (rotString.Length == 1)
        {
            rotString = "00" + rotString;
        }
        else if (rotString.Length == 2)
        {
            rotString = "0" + rotString;
            floatingText.transform.rotation = Quaternion.identity;
        }
        //Appends the text of the floating text with it's rotation.
        textmesh.text += "\n" + rotString + "°";
    }

    //Function to destroy the plane if collision is detected.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Checks if the collision is with the plane or a missile.
        if (collision.gameObject.CompareTag("Plane"))
        {
            //Debuging purposes.
            Debug.Log("Collision Detected" + collision.gameObject.name);
            //Destroys the floating text.
            Destroy(transform.GetChild(0).gameObject);
            //Moves the collision effect to the point of collision.
            GameManager.instance.collisionEffect.transform.position = collision.GetContact(0).point;
            //Turns off the alarm in the UI Input Window (In case Skynet Plane Crashed).
            GameManager.instance.UIManager.AlarmOff();
            //Stops the locked on effect (In case Skynet Plane Crashed).
            AudioManager.instance.StopSoundEffectsSound();
            //Sets the game state to GameOver.
            GameManager.instance.UpdateGameState(GameState.GameOver);
        }
        else if (collision.gameObject.CompareTag("Missile"))
        {
            //Debuging purposes.
            Debug.Log("Collision Detected" + collision.gameObject.name);
            //Moves the collision effect to the point of collision.
            GameManager.instance.collisionEffect.transform.position = collision.GetContact(0).point;
            //Turns off the alarm in the UI Input Window.
            GameManager.instance.UIManager.AlarmOff();
            //Stops the locked on effect.
            AudioManager.instance.StopSoundEffectsSound();
            //Sets the game state to GameOver.
            GameManager.instance.UpdateGameState(GameState.GameOver);
        }
    }

    private void OnBecameInvisible()
    {
        //Checks if the game state is Playing and the tag of the object is Plane.
        if ((gameObject.tag == "Plane") && ((GameManager.instance.State == GameState.Playing) || (GameManager.instance.State == GameState.EndOfRound)))
        {
            Debug.Log("Plane invisible");
            //Checks if the previous selection is the current selection.
            if (GameManager.instance.PreviousSelection == gameObject)
            {
                //This is to prevent on off screen Plane from being selected/controlable.
                GameManager.instance.PlaneDeselection();
            }
            //Destroys the plane if it goes off screen.
            Destroy(gameObject);
            //Adds 1 to the score counter.
            GameManager.instance.AddScore();
        }
    }

    private void OnBecameVisible()
    {
        //Has come into Camera View and now has a tag of Plane.
        gameObject.tag = "Plane";
        Debug.Log("Plane visible");
        //Plays the spawn sound effect.
        AudioManager.instance.PlaySpawnSound();
    }

    private void FixedUpdate()
    {
        
    }
}