using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCar : MonoBehaviour
{
    // bool value that checks whether the user has instructed the car to reach the taxi point
    //This needs to be updated later to obtain a value from a script that Jillian writes -Brennan
    //private bool isControlled = true;
    public TaxiPointSelector controller;
    public CarController car;


    // Need to add a tag check to make sure its the intersection collider specifically and not some other collider
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Intersection"))
        {
            if (car.setDest)
            {
                controller.updatePhase();
            }
        }
    }
}
