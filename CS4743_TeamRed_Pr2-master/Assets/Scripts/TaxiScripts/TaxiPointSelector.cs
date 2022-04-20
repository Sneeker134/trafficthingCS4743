using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiPointSelector : MonoBehaviour
{
    private int selected;
    public int numIntersections = 16;
    public bool hasPassenger;
    public int pawnIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the first point of interest
        selected = Random.Range(0, numIntersections - 1);
        gameObject.transform.GetChild(selected).gameObject.SetActive(true);
        gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex).gameObject.SetActive(true);
        gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex + 1).gameObject.SetActive(false);
    }

    public void updatePhase()
    {
        // If the car has a passenger then disable the current point and randomly choose another point to enable from the entire pool of points other than the one that was just enabled
        // Maybe there should be a gui counter that displays the number of passengers delivered, idk -Brennan
        int prevSelected = selected;
        if (hasPassenger)
        {
            hasPassenger = false;
            //Increment a counter or something idk
            Debug.Log("Passenger Delivered!");
        }
        // create the next point of interest which will be where the passesnger is dropped off
        else 
        {
            hasPassenger = true;
        }

        //Disable previous point
        gameObject.transform.GetChild(prevSelected).gameObject.SetActive(false);
        while (selected == prevSelected)
            selected = Random.Range(0, numIntersections - 1);

        //Enable next point
        gameObject.transform.GetChild(selected).gameObject.SetActive(true);

        //Enable the pawn or the arrow depending on whether or not a passenger has been picked up
        if (hasPassenger)
        {
            gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex).gameObject.SetActive(false);
            gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex + 1).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex).gameObject.SetActive(true);
            gameObject.transform.GetChild(selected).gameObject.transform.GetChild(pawnIndex + 1).gameObject.SetActive(false);
        }
    }
}
