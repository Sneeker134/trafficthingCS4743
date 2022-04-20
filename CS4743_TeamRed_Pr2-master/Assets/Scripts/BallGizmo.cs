using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Default,
    Destination,
    Start
}

public class BallGizmo : MonoBehaviour
{
    public float radius = 0.1f;
    public State state = State.Default;

    private void OnDrawGizmos()
    {
        // Draw a blue sphere at the transform's position
        if (state == State.Default)
            Gizmos.color = new Color(0, 0, 1, 0.5f);
        else if (state == State.Start)
            Gizmos.color = new Color(1, 0, 0, 0.5f);
        else
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
