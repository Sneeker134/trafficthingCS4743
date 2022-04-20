using UnityEngine;

public enum Direction
{
    Forward,
    Right,
    Left
}

public class CarBehavior : MonoBehaviour
{
    // Public variables
    public GameObject streets;
    public float maxSpeed = 2;
    public float maxAcceleration = 1;
    public float citySize = 0.05f;

    // Variables for waypoints
    public Waypoint curWaypoint, nxtWaypoint;
    private float t = 0;
    public Direction direction = Direction.Forward;

    // Path parameter variables
    private float lambda = 0;
    private float omega = 0;
    private float radius = 0;
    private float rx = 0;
    private float ry = 0;
    private float x0 = 0;
    private float y0 = 0;

    // Past variables (for velocity)
    private float prevX, prevZ;

    // Stop Light variables
    private LightStatus lightStatus = LightStatus.Green;

    // Car Detection
    private GameObject carDetector;
    private float length, width, height;
    private float stopDist;
    public bool brake = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set proper parameters
        if (direction != Direction.Forward)
        {
            float dx1 = curWaypoint.waypoint.transform.forward.x;
            float x1 = curWaypoint.waypoint.transform.position.x;
            float y1 = curWaypoint.waypoint.transform.position.z;
            float x2 = nxtWaypoint.waypoint.transform.position.x;
            float y2 = nxtWaypoint.waypoint.transform.position.z;

            lambda = (Mathf.Round(dx1) != 0 ? Mathf.PI / 2 : 0);
            omega = Mathf.PI / 2;
            radius = (x1 - x2) / (Mathf.Cos(lambda) - Mathf.Cos(omega + lambda));
            rx = (x1 - x2);
            ry = (Mathf.Round(dx1) != 0 ? -1 : 1) * (y2 - y1);
            x0 = (Mathf.Round(dx1) != 0 ? x1 : x2);
            y0 = (Mathf.Round(dx1) != 0 ? y2 : y1);
        }

        // Change waypoint states (for debugging)
        curWaypoint.waypoint.GetComponent<BallGizmo>().state = State.Start;
        nxtWaypoint.waypoint.GetComponent<BallGizmo>().state = State.Destination;

        // Set location data for velocity purposes
        prevX = transform.localPosition.x;
        prevZ = transform.localPosition.z;

        // Get car size
        MeshFilter mf = GetComponentInChildren(typeof(MeshFilter)) as MeshFilter;
        if (mf != null)
        {
            width = mf.mesh.bounds.size.x;
            height = mf.mesh.bounds.size.y;
            length = mf.mesh.bounds.size.z;
        }

        float stopT = maxSpeed / maxAcceleration;
        stopDist = maxSpeed * stopT - maxAcceleration / 2 * stopT * stopT;

        // Create Car Detector
        carDetector = new GameObject(name + " - Car Detector");
        carDetector.transform.position = transform.position;
        carDetector.transform.rotation = transform.rotation;
        carDetector.transform.parent = transform;
        carDetector.transform.localScale = new Vector3(width / 10, height, stopDist * 2);

        carDetector.transform.localPosition += Vector3.forward * length + Vector3.up * height;

        BoxCollider box = carDetector.AddComponent(typeof(BoxCollider)) as BoxCollider;
        box.isTrigger = true;
        carDetector.AddComponent(typeof(CarDetectorBehavior));

        maxSpeed = maxSpeed * citySize;
        maxAcceleration = maxAcceleration * citySize;

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // If parameters not defined, escape
        if (curWaypoint == null || nxtWaypoint == null) return;

        bool greenLight = lightStatus == LightStatus.Green;
        bool accelerate = greenLight;
        if (!greenLight)
        {
            float stopT = maxSpeed / maxAcceleration;
            float stopDist = maxSpeed * stopT - maxAcceleration / 2 * stopT * stopT;
            float lightDist = (nxtWaypoint.waypoint.transform.position - transform.position).magnitude;
            accelerate = lightDist * 0.5 > stopDist * 2;
        }

        // Find new positon, update previous values, and set position
        Vector3 newPos = GetNextPosition(t, accelerate && !brake);
        prevX = transform.position.x;
        prevZ = transform.position.z;
        transform.position = newPos;

        // Find and set new rotation
        Quaternion newRot = new Quaternion();
        newRot.SetLookRotation(GetDirection(t), Vector3.up);
        transform.rotation = newRot;

        // Get new waypoints if done with current set
        if (t >= 1)
        {
            ChooseNextWaypoint();
            while (t >= 1) t--;
        }
    }

    private void ChooseNextWaypoint()
    {
        // Reset state of current waypoint (for debugging)
        curWaypoint.waypoint.GetComponent<BallGizmo>().state = State.Default;

        // Set current waypoint and randomly select next waypoint
        curWaypoint = nxtWaypoint;
        int options = (curWaypoint.forward != null ? 1 : 0) + (curWaypoint.right != null ? 1 : 0) + (curWaypoint.left != null ? 1 : 0);
        if (curWaypoint.forward != null && Random.Range(0, options) == 0)
        {
            nxtWaypoint = curWaypoint.forward;
            direction = Direction.Forward;
        }
        else
        {
            options -= curWaypoint.forward != null ? 1 : 0;
            if (curWaypoint.right != null && Random.Range(0, options) == 0)
            {
                nxtWaypoint = curWaypoint.right;
                direction = Direction.Right;
            }
            else
            {
                nxtWaypoint = curWaypoint.left;
                direction = Direction.Left;
            }
        }

        // Update waypoint states (for debugging)
        curWaypoint.waypoint.GetComponent<BallGizmo>().state = State.Start;
        nxtWaypoint.waypoint.GetComponent<BallGizmo>().state = State.Destination;

        // Update path parameters
        if (direction != Direction.Forward)
        {
            float dx1 = curWaypoint.waypoint.transform.forward.x;
            float x1 = curWaypoint.waypoint.transform.position.x;
            float y1 = curWaypoint.waypoint.transform.position.z;
            float x2 = nxtWaypoint.waypoint.transform.position.x;
            float y2 = nxtWaypoint.waypoint.transform.position.z;

            lambda = (Mathf.Round(dx1) != 0 ? Mathf.PI / 2 : 0);
            omega = Mathf.PI / 2;
            radius = (x1 - x2) / (Mathf.Cos(lambda) - Mathf.Cos(omega + lambda));
            rx = (x1 - x2);
            ry = (Mathf.Round(dx1) != 0 ? -1 : 1) * (y2 - y1);
            x0 = (Mathf.Round(dx1) != 0 ? x1 : x2);
            y0 = (Mathf.Round(dx1) != 0 ? y2 : y1);
        }
    }

    private Vector3 GetNextPosition(float t, bool accelerating)
    {
        if (direction == Direction.Forward)
            this.t = t +
                GetNextSpeed(accelerating) * Time.deltaTime /
                (nxtWaypoint.waypoint.transform.position - curWaypoint.waypoint.transform.position).magnitude;
        else
            this.t = t + GetNextSpeed(accelerating) * Time.deltaTime / Mathf.Abs(radius * omega);

        return GetPosition(this.t);
    }

    private Vector3 GetPosition(float t)
    {
        if (direction == Direction.Forward)
        {
            Vector3 xzPos = (nxtWaypoint.waypoint.transform.position - curWaypoint.waypoint.transform.position) * t
                + curWaypoint.waypoint.transform.position;
            float yPos = transform.position.y;

            return new Vector3(xzPos.x, yPos, xzPos.z);
        }
        else
        {
            float xPos = x0 + rx * Mathf.Cos(omega * t + lambda);
            float zPos = y0 + ry * Mathf.Sin(omega * t + lambda);
            float yPos = transform.position.y;

            return new Vector3(xPos, yPos, zPos);
        }
    }

    private float GetNextSpeed(bool accelerating)
    {
        Vector2 prevPos = new Vector2(prevX, prevZ);
        Vector2 curPos = new Vector2(transform.position.x, transform.position.z);
        if (accelerating)
        {
            float vel = (curPos - prevPos).magnitude / Time.deltaTime + maxAcceleration * Time.deltaTime;
            return vel < maxSpeed ? vel : maxSpeed;
        }
        else
        {
            float vel = (curPos - prevPos).magnitude / Time.deltaTime - maxAcceleration * Time.deltaTime;
            return vel > 0 ? vel : 0;
        }
    }

    private Vector3 GetDirection(float t)
    {
        if (direction == Direction.Forward)
        {
            return nxtWaypoint.waypoint.transform.forward;
        }
        else
        {
            return new Vector3(-rx * Mathf.Sin(omega * t + lambda), 0, ry * Mathf.Cos(omega * t + lambda)).normalized;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == carDetector) return;
        LightDetectorBehavior lightScript = other.GetComponent<LightDetectorBehavior>();
        CarBehavior carScript = other.GetComponent<CarBehavior>();
        CarController carController = other.GetComponent<CarController>();
        if (lightScript != null)
            lightStatus = lightScript.lightStatus;
        else if (carScript != null)
            brake = true;
        else if (carController != null)
            brake = true;
        //print("Car: Light Status = " + lightStatus + ", Dist: " + (lightScript.lightPos - transform.position));
    }

    private void OnTriggerExit(Collider other)
    {
        lightStatus = LightStatus.Green;
        brake = false;
    }
}
