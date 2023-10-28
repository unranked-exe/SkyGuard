using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmScript : MonoBehaviour
{
    //References to the alarm images
    [SerializeField] private Image alarm1;
    [SerializeField] private Image alarm2;

    //Turns the alarm images red
    public void AlarmOn()
    {
        alarm1.color = Color.red;
        alarm2.color = Color.red;
    }

    //Turns the alarm images black
    public void AlarmOff()
    {
        alarm1.color = Color.black;
        alarm2.color = Color.black;
    }
}
