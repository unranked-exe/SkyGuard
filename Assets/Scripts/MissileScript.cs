using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    //Variable for rigidbody of missile
    [SerializeField] private Rigidbody2D rb;

    //Variable for speed of missile
    [SerializeField] private float _speed = 2f;
    //Variable for rotation speed of missile
    [SerializeField] private float _rotationSpeed = 200f;

    //Variable for target of missile
    [SerializeField] private Transform _target;

    //Function to set the target of the missile
    public void SetTargetPlane(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        //If the target is in the scene.
        if (_target != null)
        {
            //Gets the direction to the target.
            Vector2 direction = (Vector2)_target.position - rb.position;
            //Normalizes the direction.
            direction.Normalize();
            //Caluclates amount to rotate by.
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            //Rotates missile towards target.
            rb.angularVelocity = -_rotationSpeed * rotateAmount;
            //Moves missile forward.
            rb.velocity = transform.up * _speed;
        }
        else
        {
            //Destroy missile if target is null
            Destroy(gameObject);
        }
    }
}
