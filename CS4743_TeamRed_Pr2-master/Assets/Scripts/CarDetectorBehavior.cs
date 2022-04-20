using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDetectorBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == transform.parent) return;
        if (other.transform.parent == null) return;

        CarBehavior carScript = other.transform.parent.GetComponent<CarBehavior>();
        if (carScript != null)
        {
            carScript = transform.parent.GetComponent<CarBehavior>();
            carScript.brake = true;
        }
        // print(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform.parent) return;
        if (other.transform.parent == null) return;

        CarBehavior carScript = other.transform.parent.GetComponent<CarBehavior>();
        if (carScript != null)
        {
            carScript = transform.parent.GetComponent<CarBehavior>();
            carScript.brake = false;
        }
    }
}
