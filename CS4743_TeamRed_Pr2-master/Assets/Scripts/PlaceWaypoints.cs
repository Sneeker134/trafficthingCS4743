using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IntersectionType
{
    L,
    T,
    X
}

public struct Intersection
{
    public GameObject gameObject;
    public int row, col;
    public IntersectionType type;
    public Waypoint S_SN, N_SN, W_WE, E_WE, N_NS, S_NS, E_EW, W_EW;
    public float rotation;
}

public class Waypoint
{
    public GameObject waypoint;
    public Waypoint forward, left, right;
}

public class PlaceWaypoints : MonoBehaviour
{
    public GameObject Waypoint;

    public float centerRoad = 0.1f;
    public Intersection[,] intersections;
    public bool loaded = false;
    private bool loading = false;

    public float citySize = 0.05f;

    public void Load()
    {
        if (loading) return;
        loading = true;

        // Find Intersections
        int n = 0;
        intersections = new Intersection[n, n];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "Intersections")
            {
                n = Mathf.CeilToInt(Mathf.Sqrt(transform.GetChild(i).childCount));
                intersections = new Intersection[n, n];
                
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    // Expecting name to be of the form: "<name> <row><col> <Type>"
                    string[] name = transform.GetChild(i).GetChild(j).name.Split(' ');
                    int row = int.Parse("" + name[1][0]) - 1;
                    int col = int.Parse("" + name[1][1]) - 1;
                    IntersectionType type = name[2] == "L" ?
                        IntersectionType.L :
                        name[2] == "T" ? IntersectionType.T : IntersectionType.X;
                    float rotation = transform.GetChild(i).GetChild(j).rotation.eulerAngles.y;
                    while (rotation < 0) rotation += 360;
                    rotation = Mathf.Round(rotation / 90) * 90;

                    intersections[row, col] = new Intersection
                    {
                        gameObject = transform.GetChild(i).GetChild(j).gameObject,
                        row = row,
                        col = col,
                        type = type,
                        rotation = rotation
                    };
                }
                break;
            }
        }
        
        // NOTE: North is positive z direction (forward)

        // Instantiate Waypoints
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                Intersection intersection = intersections[i, j];
                MeshFilter mr = intersection.gameObject.GetComponent<MeshFilter>();
                if (mr == null) continue;
                
                float length = mr.mesh.bounds.size.x;
                float width = mr.mesh.bounds.size.z;
                float height = mr.mesh.bounds.size.y;

                // NOTE: North is local positive z direction (forward)

                // Waypoints for all Intersection Types (L, T, X)

                // North Waypoints
                if ((intersection.type == IntersectionType.L && (intersection.rotation == 0 || intersection.rotation == 90)) ||
                    (intersection.type == IntersectionType.T && (intersection.rotation != 270)) ||
                    intersection.type == IntersectionType.X)
                {
                    // North waypoint going from South to North
                    intersection.N_SN = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.N_SN.waypoint.transform.position += new Vector3(length * centerRoad * citySize, (height / 2) * citySize, (width / 2) * citySize);
                    intersection.N_SN.waypoint.transform.rotation = Quaternion.Euler(Vector3.zero);

                    // North waypoint going from North to South
                    intersection.N_NS = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.N_NS.waypoint.transform.position += new Vector3(-length * centerRoad * citySize, (height / 2) * citySize, (width / 2) * citySize);
                    intersection.N_NS.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 180);
                }

                // East Waypoints
                if ((intersection.type == IntersectionType.L && (intersection.rotation == 90 || intersection.rotation == 180)) ||
                    (intersection.type == IntersectionType.T && (intersection.rotation != 0)) ||
                    intersection.type == IntersectionType.X)
                {
                    // East waypoint going from West to East
                    intersection.E_WE = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.E_WE.waypoint.transform.position += new Vector3((length / 2) * citySize, (height / 2) * citySize, (-width * centerRoad) * citySize);
                    intersection.E_WE.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 90);

                    // East waypoint going from West to East
                    intersection.E_EW = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.E_EW.waypoint.transform.position += new Vector3((length / 2) * citySize, (height / 2) * citySize, (width * centerRoad) * citySize);
                    intersection.E_EW.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 270);
                }

                // South Waypoints
                if ((intersection.type == IntersectionType.L && (intersection.rotation == 180 || intersection.rotation == 270)) ||
                    (intersection.type == IntersectionType.T && (intersection.rotation != 90)) ||
                    intersection.type == IntersectionType.X)
                {
                    // South waypoint going from North to South
                    intersection.S_NS = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.S_NS.waypoint.transform.position += new Vector3((-length * centerRoad) * citySize, (height / 2) * citySize, (-width / 2) * citySize);
                    intersection.S_NS.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 180);

                    // South waypoint going from South to North
                    intersection.S_SN = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.S_SN.waypoint.transform.position += new Vector3((length * centerRoad) * citySize, (height / 2) * citySize, (-width / 2) * citySize);
                    intersection.S_SN.waypoint.transform.rotation = Quaternion.Euler(Vector3.zero);
                }

                // West Waypoints
                if ((intersection.type == IntersectionType.L && (intersection.rotation == 270 || intersection.rotation == 0)) ||
                    (intersection.type == IntersectionType.T && (intersection.rotation != 180)) ||
                    intersection.type == IntersectionType.X)
                {
                    // West waypoint going from West to East
                    intersection.W_WE = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.W_WE.waypoint.transform.position += new Vector3((-length / 2) * citySize, (height / 2) * citySize, (-width * centerRoad) * citySize);
                    intersection.W_WE.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 90);

                    // West waypoint going from East to West
                    intersection.W_EW = new Waypoint
                    {
                        waypoint = GameObject.Instantiate(Waypoint, intersection.gameObject.transform)
                    };
                    intersection.W_EW.waypoint.transform.position += new Vector3((-length / 2) * citySize, (height / 2) * citySize, (width * centerRoad) * citySize);
                    intersection.W_EW.waypoint.transform.rotation = Quaternion.Euler(Vector3.up * 270);
                }

                intersections[i, j] = intersection;
            }

        // NOTE: North is further collumns, East is further rows

        // Connect Waypoints
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                Intersection intersection = intersections[i, j];

                // Internal Connections
                if (intersection.S_SN != null)
                {
                    intersection.S_SN.forward = intersection.N_SN;
                    intersection.S_SN.right = intersection.E_WE;
                    intersection.S_SN.left = intersection.W_EW;
                }
                if (intersection.W_WE != null)
                {
                    intersection.W_WE.forward = intersection.E_WE;
                    intersection.W_WE.right = intersection.S_NS;
                    intersection.W_WE.left = intersection.N_SN;
                }
                if (intersection.N_NS != null)
                {
                    intersection.N_NS.forward = intersection.S_NS;
                    intersection.N_NS.right = intersection.W_EW;
                    intersection.N_NS.left = intersection.E_WE;
                }
                if (intersection.E_EW != null)
                {
                    intersection.E_EW.forward = intersection.W_EW;
                    intersection.E_EW.right = intersection.N_SN;
                    intersection.E_EW.left = intersection.S_NS;
                }

                // External Connections
                if (intersection.N_SN != null && j < n - 1)
                    intersection.N_SN.forward = intersections[i, j + 1].S_SN;
                if (intersection.E_WE != null && i < n - 1)
                    intersection.E_WE.forward = intersections[i + 1, j].W_WE;
                if (intersection.S_NS != null && j > 0)
                    intersection.S_NS.forward = intersections[i, j - 1].N_NS;
                if (intersection.W_EW != null && i > 0)
                    intersection.W_EW.forward = intersections[i - 1, j].E_EW;

                intersections[i, j] = intersection;
            }

        loaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
