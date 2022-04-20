using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightStatus
{
    Red,
    Yellow,
    Green
}

public class LightDetectorBehavior : MonoBehaviour
{
    public LightStatus lightStatus;
    public Vector3 lightPos;

    private GameObject green, yellow, red;

    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child != transform)
            {
                switch (child.name.ToLower())
                {
                    case "redlight":
                        red = child.gameObject;
                        break;
                    case "greenlight":
                        green = child.gameObject;
                        break;
                    case "yellowlight":
                        yellow = child.gameObject;
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (red != null && red.activeSelf)
            lightStatus = LightStatus.Red;
        else if (yellow != null && yellow.activeSelf)
            lightStatus = LightStatus.Yellow;
        else if (green != null && green.activeSelf)
            lightStatus = LightStatus.Green;
        else
            print("Light Status Error...");
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
