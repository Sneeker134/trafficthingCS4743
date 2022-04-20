using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSBoxSensor : MonoBehaviour
{
    public bool canGo = true;

    public GameObject lightss; 


    void Update()
    {

        // if up and down lights are green, car can go, else can't go
        if (lightss.GetComponent<Streetlight>().direction == signal.UpDown)
        {
            canGo = true;
        }
        else
        {
            canGo = false; 
        }


        // if canGo is true, turn off box collider of light sensor, else keep collider on
        if (canGo == true)
        {
            // car can go
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            // car can't go
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        
    }
}