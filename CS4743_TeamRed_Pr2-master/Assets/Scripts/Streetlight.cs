using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//UpDown indicates North and South are the green lights. UpDownSlow indicates it has changed to a yellow light. LeftRight and LeftRight does the same for the East and West pairing.
public enum signal
{
    UpDown,
    UpDownSlow,
    LeftRight,
    LeftRightSlow
}
public class Streetlight : MonoBehaviour
{
    //These are the streetlight objects themselves, and information they hold. To fetch what state the interestion is actually at, use the public signal direction.
    private GameObject north;
    private GameObject south;
    private GameObject east;
    private GameObject west;
    public signal direction = signal.UpDown;
    float timer;
    lightHolder northHold;
    lightHolder southHold;
    lightHolder eastHold;
    lightHolder westHold;

    //Minimum and Maximum times show the range a light could stay green. slowTime shows how long a yellow light lasts.
    public float minimumTime = 5.0f;
    public float maximumTime = 10.0f;
    public float slowTime = 2.0f;

    struct lightHolder
    {
        public GameObject red;
        public GameObject yellow;
        public GameObject green;

        public void setRed(GameObject temp)
        {
            this.red = temp;
        }
        public void setYellow(GameObject temp)
        {
            this.yellow = temp;
        }
        public void setGreen(GameObject temp)
        {
            this.green = temp;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject temp;
        //Assign each light to its respective gameobject
        foreach (Transform eachChild in transform)
        {
            if (eachChild.GetSiblingIndex() == 0)
            {
                north = eachChild.gameObject;
                temp = north.transform.GetChild(0).gameObject;
                northHold.setRed(temp);
                temp = north.transform.GetChild(1).gameObject;
                northHold.setGreen(temp);
                temp = north.transform.GetChild(2).gameObject;
                northHold.setYellow(temp);

            } else if (eachChild.GetSiblingIndex() == 3)
            {
                south = eachChild.gameObject;
                temp = south.transform.GetChild(0).gameObject;
                southHold.setRed(temp);
                temp = south.transform.GetChild(1).gameObject;
                southHold.setGreen(temp);
                temp = south.transform.GetChild(2).gameObject;
                southHold.setYellow(temp);
            }
            else if (eachChild.GetSiblingIndex() == 1)
            {
                east = eachChild.gameObject;
                temp = east.transform.GetChild(0).gameObject;
                eastHold.setRed(temp);
                temp = east.transform.GetChild(1).gameObject;
                eastHold.setGreen(temp);
                temp = east.transform.GetChild(2).gameObject;
                eastHold.setYellow(temp);
            }
            else if (eachChild.GetSiblingIndex() == 2)
            {
                west = eachChild.gameObject;
                temp = west.transform.GetChild(0).gameObject;
                westHold.setRed(temp);
                temp = west.transform.GetChild(1).gameObject;
                westHold.setGreen(temp);
                temp = west.transform.GetChild(2).gameObject;
                westHold.setYellow(temp);

            }

        }

        //Make sure assigning was successful.
        if (north != null && west != null && east != null && south != null)
        {
            Debug.Log("Streetlights assigned.");
        } else if(north == null)
        {
            Debug.Log("Unable to find north");
        }
        else if (south == null)
        {
            Debug.Log("Unable to find south");
        }
        else if (west == null)
        {
            Debug.Log("Unable to find west.");
        }
        else if (east == null)
        {
            Debug.Log("Unable to find east.");
        }

        //Set timer
        timer = Random.Range(minimumTime, maximumTime);

    }

    // Update is called once per frame
    void Update()
    {
        //Count down the timer. When it hits 0, check the state to know which lights to switch and which enum to set to.
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        } else
        {
            Debug.Log("Switching State.");
            if(direction == signal.UpDown)
            {
                northHold.green.SetActive(false);
                northHold.yellow.SetActive(true);
                southHold.green.SetActive(false);
                southHold.yellow.SetActive(true);
                timer = slowTime;
                direction = signal.UpDownSlow;
            } else if(direction == signal.UpDownSlow)
            {
                northHold.red.SetActive(true);
                northHold.yellow.SetActive(false);
                southHold.red.SetActive(true);
                southHold.yellow.SetActive(false);

                westHold.red.SetActive(false); ;
                westHold.green.SetActive(true);
                eastHold.red.SetActive(false);
                eastHold.green.SetActive(true);
                timer = Random.Range(minimumTime, maximumTime);
                direction = signal.LeftRight;
            } else if (direction == signal.LeftRight)
            {
                eastHold.green.SetActive(false);
                eastHold.yellow.SetActive(true);
                westHold.green.SetActive(false);
                westHold.yellow.SetActive(true);
                timer = slowTime;
                direction = signal.LeftRightSlow;
            }
            else if (direction == signal.LeftRightSlow)
            {
                eastHold.red.SetActive(true);
                eastHold.yellow.SetActive(false); ;
                westHold.red.SetActive(true);
                westHold.yellow.SetActive(false);

                northHold.red.SetActive(false);
                northHold.green.SetActive(true);
                southHold.red.SetActive(false);
                southHold.green.SetActive(true);
                timer = Random.Range(minimumTime, maximumTime);
                direction = signal.UpDown;
            }
        }
    }

}
