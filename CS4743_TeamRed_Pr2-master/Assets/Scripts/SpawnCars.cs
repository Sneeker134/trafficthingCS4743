using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public GameObject[] toSpawn;
    public int numberOfCars = 10;

    private GameObject cars;
    private GameObject streets;

    // Start is called before the first frame update
    void Start()
    {
        var script = GetComponentInChildren(typeof(PlaceWaypoints)) as PlaceWaypoints;
        streets = script.gameObject;
        script.Load();
        
        cars = new GameObject("Cars");
        Intersection[,] intersections = script.intersections;
        int n = intersections.GetLength(0);
        int m = intersections.GetLength(1);
        int rowcol = n * m;
        //List<Intersection> intList = new List<Intersection>(n*m);
        //for (int i = 0; i < n; i++)
        //    for (int j = 0; j < m; j++)
        //        intList.Add(intersections[i, j]);

        List<Waypoint> waypoints = new List<Waypoint>();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
            {
                var intersection = intersections[i, j];
                if (intersection.N_NS != null) waypoints.Add(intersection.N_NS);
                if (intersection.N_SN != null) waypoints.Add(intersection.N_SN);
                if (intersection.E_EW != null) waypoints.Add(intersection.E_EW);
                if (intersection.E_WE != null) waypoints.Add(intersection.E_WE);
                if (intersection.S_NS != null) waypoints.Add(intersection.S_NS);
                if (intersection.S_SN != null) waypoints.Add(intersection.S_SN);
                if (intersection.W_WE != null) waypoints.Add(intersection.W_WE);
                if (intersection.W_EW != null) waypoints.Add(intersection.W_EW);
            }

        for (int i = 0; i < numberOfCars; i++)
        {
            int index = Random.Range(0, waypoints.Count);
            SpawnCarAtWaypoint(waypoints[index], i);
            waypoints.RemoveAt(index);
        }
    }

    private void SpawnCarAtWaypoint(Waypoint wp, int number)
    {
        GameObject car = GameObject.Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], cars.transform);
        car.name = "Car " + number;
        car.transform.position = wp.waypoint.transform.position;
        car.transform.rotation = wp.waypoint.transform.rotation;
        CarBehavior script = car.GetComponent<CarBehavior>();
        script.streets = streets;
        script.curWaypoint = wp;
        
        int options = (wp.forward != null ? 1 : 0) + (wp.right != null ? 1 : 0) + (wp.left != null ? 1 : 0);
        if (wp.forward != null && Random.Range(0, options) == 0)
        {
            script.nxtWaypoint = wp.forward;
            script.direction = Direction.Forward;
        }
        else
        {
            options -= wp.forward != null ? 1 : 0;
            if (wp.right != null && Random.Range(0, options) == 0)
            {
                script.nxtWaypoint = wp.right;
                script.direction = Direction.Right;
            }
            else
            {
                script.nxtWaypoint = wp.left;
                script.direction = Direction.Left;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
