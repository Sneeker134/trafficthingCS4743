using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCityMap : MonoBehaviour
{
    public GameObject CityMap;

    public void openCityMap()
    {
        if(CityMap != null)
        {
            bool isActive = CityMap.activeSelf;

            CityMap.SetActive(!isActive);
        }
    }
}
