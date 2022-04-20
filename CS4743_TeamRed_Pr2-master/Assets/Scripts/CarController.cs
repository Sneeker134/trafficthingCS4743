using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class CarController : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public bool setDest;
    public Transform destination;
    public bool isBraking = false;
    public float extraRotationSpeed = 5f;
    public float sensorLength = 0.7f;
    public Vector3 frontSensorPos = new Vector3(.02f, 0.003f, 0.013f);
    public Vector3 frontLeftSensorPos = new Vector3(0f, 0.003f, 0.013f);
    public Vector3 centerSensorPos = new Vector3(0.01f, 0.003f, 0.013f);


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    // raycast sensor at front of car to detect objects and brake if needed
    private void Sensors()
    {
        RaycastHit hit;
        // get the sensor start position to be where the very front of the car is
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        sensorStartPos += transform.right * frontSensorPos.x;
        isBraking = false;

        // put out a raycast if ray hits anything tagged car, will brake
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isBraking = true;
            }

        }


        Vector3 sensorLeftStartPos = transform.position;
        sensorLeftStartPos += transform.forward * frontLeftSensorPos.z;
        sensorLeftStartPos += transform.up * frontLeftSensorPos.y;
        sensorLeftStartPos += transform.right * frontLeftSensorPos.x;
        // isBraking = false;

        // Debug.DrawLine(sensorLeftStartPos, hit.point);
        if (Physics.Raycast(sensorLeftStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorLeftStartPos, hit.point);
                isBraking = true;
            }
        }

        Vector3 centerStartPos = transform.position;
        centerStartPos += transform.forward * centerSensorPos.z;
        centerStartPos += transform.up * centerSensorPos.y;
        centerStartPos += transform.right * centerSensorPos.x;
        // isBraking = false;

        // Debug.DrawLine(sensorLeftStartPos, hit.point);
        if (Physics.Raycast(centerStartPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(centerStartPos, hit.point);
                isBraking = true;
            }
        }

    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    // make the car go to a specific position
    private void SetDestination()
    {
        Vector3 targetVector = destination.transform.position;
        agent.SetDestination(targetVector);

    }

    // if isBraking is true, make agent/car stop, else go
    private void Braking()
    {
        if (isBraking)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    // make agent turn faster
    void extraRotation()
    {
        Vector3 lookrotation = agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

    }

    /*
    // car stops if it hits anything called car, like a car accidents
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "car")
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }
    }
    */

    void Update()
    {

        // if a setDest is true and a destination object is in place, go to the destination,
        if (setDest && destination != null)
        {
            SetDestination();
            setDest = false;
        }
        // else keep on preset path
        else
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
        Sensors();
        extraRotation();
        Braking();
    }
}