using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSelection : MonoBehaviour
{
    public void selectDestination(int destination)
    {
        GameObject car = GameObject.Find("points");
        CarController carController = car.GetComponent<CarController>();
        Transform temp = carController.points[destination];
        carController.destination = temp;
    }
}
