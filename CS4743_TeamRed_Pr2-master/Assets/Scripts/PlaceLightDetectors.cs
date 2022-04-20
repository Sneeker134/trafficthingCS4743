using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceLightDetectors : MonoBehaviour
{
    private float length = 3;
    private float offset = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshFilter>().mesh.bounds.size;
        float height = size.y;
        float width = size.x;
        GameObject boxObj = new GameObject();
        BoxCollider collider = boxObj.AddComponent(typeof(BoxCollider)) as BoxCollider;
        collider.isTrigger = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject streetlight = transform.GetChild(i).gameObject;
            for (int j = 0; j < streetlight.transform.childCount && j < 4; j++)
            {
                GameObject light = streetlight.transform.GetChild(j).gameObject;
                if (light.activeSelf)
                {
                    GameObject box = GameObject.Instantiate(boxObj, light.transform);
                    box.transform.localScale = new Vector3(width / 10, height / 2, length);
                    box.transform.localPosition += Vector3.right * 7*width/8;
                    Vector3 lightPos = box.transform.position;
                    box.transform.localPosition += -Vector3.forward * (offset + length / 2);

                    LightDetectorBehavior script = box.AddComponent(typeof(LightDetectorBehavior)) as LightDetectorBehavior;
                    script.lightPos = lightPos;
                }
            }
        }
        GameObject.Destroy(boxObj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
