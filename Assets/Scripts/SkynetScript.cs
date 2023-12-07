using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkynetScript : PlaneMoveScript
{
    //Enum to hold the states of Skynet.
    private enum SkynetState
    {
        Idle,
        Chasing,
    }

    //Variable to hold the current state of Skynet.
    [SerializeField] private SkynetState _state;

    //Run after Awake and before Start.
    private void OnEnable()
    {
        //Set the name of the plane to Skynet.
        gameObject.name = "Skynet";
        //Called to set the state of Skynet to Idle.
        updateSkynetState(SkynetState.Idle);
    }

    //Updates the state of Skynet.
    private void updateSkynetState(SkynetState newState)
    {
        switch (newState)
        {
            case SkynetState.Idle:
                _state = SkynetState.Idle;
                break;
            case SkynetState.Chasing:
                _state = SkynetState.Chasing;
                break;
            default:
                break;
        }
    }
}

