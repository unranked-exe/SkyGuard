using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMoveScript : MonoBehaviour
{
    //Reference to the Rigidbody2D component of the plane
    [SerializeField] private Rigidbody2D _PlaneRB;
    //Reference to the speed of the plane
    [SerializeField] private float _PlaneSpeed;
    private void Awake()
    {
        //Call the MovePlane function
        MovePlane();
    }

    //Function to move the plane upright
    void MovePlane()
    {
        //Set the velocity of the plane to the up direction multiplied by the speed
        _PlaneRB.velocity = transform.up * _PlaneSpeed;
    }
}
