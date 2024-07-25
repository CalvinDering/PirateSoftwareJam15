using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour, IInteractable {

    [SerializeField] private Waypoint[] allWaypoints;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody npcRB;

    private Waypoint currentWaypoint = null;
    private Vector3 targetPosition = Vector3.zero;

    private float waitingTime;
    private float waitingTimer;
    private float targetAngle;

    private void Awake() {
        npcRB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if(waitingTimer > 0) {
            waitingTimer -= Time.fixedDeltaTime;
            return;
        }

        CheckNextWaypoint();

        LookAtWaypoint();

        if(targetAngle <= 0.1f) {
            npcRB.AddForceAtPosition(transform.forward * movementSpeed, transform.position, ForceMode.Acceleration);
        }
    }

    private void CheckNextWaypoint() {
        if(currentWaypoint == null) {
            currentWaypoint = GetClosestWaypoint();
        }
        if(currentWaypoint != null) {
            targetPosition = currentWaypoint.transform.position;
            targetPosition.y = transform.position.y;

            float distanceToWaypoint = (targetPosition - transform.position).magnitude;

            if(distanceToWaypoint <= currentWaypoint.minDistanceToReachWaypoint) {
                waitingTime = currentWaypoint.waitingTime;
                waitingTimer = waitingTime;
                currentWaypoint = currentWaypoint.nextWaypoint[Random.Range(0, currentWaypoint.nextWaypoint.Length)];
            }

        }
    }

    private Waypoint GetClosestWaypoint() {
        return allWaypoints.OrderBy(w => Vector3.Distance(transform.position, w.transform.position)).FirstOrDefault();
    }

    private void LookAtWaypoint() {
        Vector3 directionToTarget = currentWaypoint.transform.position - transform.position;
        directionToTarget.y = 0.0f;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.fixedDeltaTime * turnSpeed);
        targetAngle = Quaternion.Angle(transform.rotation, lookRotation);

        if(targetAngle <= 0.1f) {
            transform.rotation = lookRotation;
        }
    }

    public void Interact(GameObject interactor) {

    }
}
