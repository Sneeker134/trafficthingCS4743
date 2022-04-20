using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    public bool on = true;
    public float increment = 1;
    private const float minValue = 1e-8f;

    private void Update()
    {
        if (on)
        {
            Vector3 newPosition = new Vector3(
                Mathf.RoundToInt(transform.localPosition.x / increment) * increment,
                Mathf.RoundToInt(transform.localPosition.y / increment) * increment,
                Mathf.RoundToInt(transform.localPosition.z / increment) * increment
            );
            transform.localPosition = newPosition;
        }
    }

    private void OnValidate()
    {
        if (increment < minValue)
        {
            increment = minValue;
        }
    }
}
