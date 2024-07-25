using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public float minDistanceToReachWaypoint = 1f;

    public Waypoint[] nextWaypoint;
    public float waitingTime;

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToReachWaypoint);
    }
}
