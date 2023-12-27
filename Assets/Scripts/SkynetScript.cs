using System.Collections;
using UnityEngine;

public class SkynetScript : PlaneMoveScript
{
    //Enum to hold the states of Skynet.
    private enum SkynetState
    {
        SpawnedIn,
        Idle,
        Chasing,
        MissileLocking,
        MissileOut,
        Despawn
    }

    //Variable to hold the current state of Skynet.
    [SerializeField] private SkynetState _state;

    //Variable to hold the nearest collider to Skynet.
    [SerializeField] private Collider2D _planeInRange;
    //Variable to hold the target plane to chase.
    [SerializeField] private GameObject targetPlane;

    //Variable to hold the radar range of Skynet.
    [SerializeField] private float _radarRange = 5f;
    //Variable to hold the layer mask of Skynet.
    [SerializeField] private LayerMask mask;

    //Run after Awake and before Start.
    private void OnEnable()
    {
        //Set the name of the plane to Skynet.
        gameObject.name = "Skynet";
        //Called to set the state of Skynet to SpawnedIn.
        updateSkynetState(SkynetState.SpawnedIn);
        //Set the layer mask to the Game Layer.
        mask = LayerMask.GetMask("Game Layer");
    }

    //Updates the state of Skynet.
    private void updateSkynetState (SkynetState skynetState)
    {
        //Set the state of Skynet to the passed in state.
        _state = skynetState;
        //Switch statement to handle the different states of Skynet.
        switch (_state)
        {
            //If the state is SpawnedIn.
            case SkynetState.SpawnedIn:
                //Call the SpawnedIn function.
                HandleSpawnedInState();
                break;
            //If the state is Idle.
            case SkynetState.Idle:
                //Call the Idle function.
                HandleIdleState();
                break;
            //If the state is Chasing.
            case SkynetState.Chasing:
                //Call the Chasing function.
                HandleChasingState();
                break;
            //If the state is MissileLocking.
            case SkynetState.MissileLocking:
                //Call the MissileLocking function.
                //HandleMissileLockingState();
                break;
            //If the state is MissileOut.
            case SkynetState.MissileOut:
                //Call the MissileOut function.
                //HandleMissileOutState();
                break;
            //If the state is Despawn.
            case SkynetState.Despawn:
                //Call the Despawn function.
                //HandleDespawnState();
                break;
        }
    }

    void HandleSpawnedInState()
    {
        //Starts the coroutine to wait for 12 seconds.
        StartCoroutine(PlaneDetectionDelay());
    }

    void HandleIdleState()
    {
        //Stops the coroutine to wait for 12 seconds.
        StopCoroutine(PlaneDetectionDelay());
        //If the plane is not at the target position.
        StartCoroutine(RadarPing());
    }

    void HandleChasingState()
    {
        StopCoroutine(RadarPing());


        TurnToAttack();
    }


    IEnumerator PlaneDetectionDelay()
    {
        //Wait for 12 seconds.
        yield return new WaitForSeconds(12f);
        //Set the state of the AI to Idle to allow it to start detection of planes.
        updateSkynetState(SkynetState.Idle);
    }


    IEnumerator RadarPing()
    {
        //Checks if the game state is playing and the state of the AI is Idle.
        while ((GameManager.instance.State == GameState.Playing) && (_state == SkynetState.Idle))
        {
            //Performs a 360 search of any collider object within radius of AI.
            _planeInRange = Physics2D.OverlapCircle(transform.position, _radarRange, mask);
            Debug.Log("Radar Ping");
            //Checks whether there is a plane in range and if it is active.
            if ((_planeInRange != null) && (_planeInRange.gameObject.tag == "Plane"))
            {
                //Assigns the plane in range to become the target plane.
                targetPlane = _planeInRange.gameObject;
                //Updates the state of the AI to Chasing.
                updateSkynetState(SkynetState.Chasing);
                Debug.Log("Plane in range");
            }
            else
            {
                Debug.Log("Plane not in range");
                //Resets the target plane to null.
                _planeInRange = null;
            }
            //Wait for 2 seconds before performing another search.
            yield return new WaitForSeconds(2f);
        }
        yield break;
    }


    private void TurnToAttack()
    {
        //Checks if the game state is playing and the state of the AI is Chasing.
        if ((GameManager.instance.State == GameState.Playing) && (_state == SkynetState.Chasing))
        {
            //Checks if the target plane is visible in the scene.
            if (targetPlane.tag == "Plane")
            {
                //Calculates the direction of the target plane.
                Vector2 direction = (targetPlane.transform.position - transform.position).normalized;
                //Calculates the rotation of the AI to face the target plane.
                float rotationAngle = Vector2.SignedAngle(transform.up, direction);
                Debug.Log(rotationAngle);
                //Gets the current rotation of the AI.
                float bearing = transform.rotation.eulerAngles.z;
                //Converts the bearing to its true bearing.
                bearing = 360 - bearing;
                Debug.Log(bearing);
                //Calculates a new bearing for the AI to face the target plane.
                rotationAngle = bearing - rotationAngle;
                Debug.Log(rotationAngle);
                //Convert the true bearing to Unity's bearing.
                rotationAngle = 360 - rotationAngle;
                //Rotates the AI to face the target plane.
                StartRotate(rotationAngle);
            }
            else
            {
                //Updates the state of the AI to Idle.
                updateSkynetState(SkynetState.Idle);
            }
        }
    }

    /*private void TurnToAttack()
    {
        
    StartRotate(360 - rotationAngle);
    
    
    //Checks if the game state is playing and the state of the AI is Chasing.
        while ((GameManager.instance.State == GameState.Playing) && (_state == SkynetState.Chasing))
        {
            //Checks if the target plane is visible in the scene.
            if (targetPlane.tag == "Plane")
            {
                //Calculates the direction of the target plane.
                Vector2 direction = (targetPlane.transform.position - transform.position).normalized;
                //Calculates the rotation of the AI to face the target plane.
                float rotationSteer = Vector2.SignedAngle(transform.up, direction);
                Debug.Log(rotationSteer);
                //Rotates the AI to face the target plane.
                int rot = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
                //Calcualtes the real bearing of the plane
                //rot = 360 - rot;
                rotationSteer = rot - rotationSteer;
                Debug.Log(rotationSteer);
                StartRotate(rotationSteer);
                updateSkynetState(SkynetState.Idle);
            }
            else
            {
                //Updates the state of the AI to Idle.
                updateSkynetState(SkynetState.Idle);
            }
        }
    }*/

    /*private void FixedUpdate()
    {
    if ((_state == SkynetState.Chasing) && (targetPlane != null))
        MovePlane();
        Vector2 direction = (targetPlane.transform.position - transform.position).normalized;
        float rotationSteer = Vector2.Dot(direction, transform.up);
        Debug.Log(rotationSteer);
        //_PlaneRB.angularVelocity = -rotationSteer * 75f;
        _PlaneSpeed += 0.001f;
        OutputBearing();
    }*/
}

