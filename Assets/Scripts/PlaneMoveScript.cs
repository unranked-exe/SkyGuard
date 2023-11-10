using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        //Set the rotation of the Floating Text to upright
        floatingText.transform.rotation = Quaternion.identity;
        OutputBearing();
        //Call the MovePlane function
        MovePlane();
    }

    //Function to move the plane upright
    void MovePlane()
    {
        //Set the velocity of the plane to the up direction multiplied by the speed
        _PlaneRB.velocity = transform.up * _PlaneSpeed;
    }

    //Function to output the bearing of the plane for Flaoting Text
    void OutputBearing()
    {
        //Get the rotation of the plane
        int rot = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
        //Calcualtes the real bearing of the plane
        rot = 360 - rot;
        //Set the text of the floating text to the rotation
        textmesh.text = rot.ToString() + "°";
    }
}
/* Debug.Log(rot);
        rot = 360 - rot;
        Debug.Log(rot);*/