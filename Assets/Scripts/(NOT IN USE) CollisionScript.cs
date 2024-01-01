using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    Animator anim;
    public void collision(Vector3 CollidePos)
    {
        transform.position = CollidePos;
        anim.Play("Collision");
    }
}
