using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC : MonoBehaviour, IInteractable {

    [SerializeField] private Waypoint[] allWaypoints;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    private Rigidbody npcRB;
    protected Animator animator;

    private Waypoint currentWaypoint = null;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 originTargetPosition;

    private bool pauseMoveing = false;
    private float waitingTime;
    private float waitingTimer;
    private float targetAngle;

    private void Start() {
        npcRB = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        originTargetPosition = transform.position + transform.forward;
    }

    private void FixedUpdate() {

        float moving = 0f;

        if(waitingTimer > 0) {
            waitingTimer -= Time.fixedDeltaTime;
        } else {
            if(!pauseMoveing) {
                CheckNextWaypoint();
                LookAtTarget();

                if(currentWaypoint != null) {
                    if(targetAngle <= 0.1f) {
                        npcRB.AddForceAtPosition(transform.forward * movementSpeed, transform.position, ForceMode.Acceleration);
                        moving = npcRB.velocity.magnitude;
                    }
                }
            } else {
                LookAtTarget();
            }
        }

        animator.SetFloat("moving", moving);
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
        } else {
            targetPosition = originTargetPosition;
        }
    }

    private Waypoint GetClosestWaypoint() {
        return allWaypoints.OrderBy(w => Vector3.Distance(transform.position, w.transform.position)).FirstOrDefault();
    }

    private void LookAtTarget() {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0.0f;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.fixedDeltaTime * turnSpeed);
        targetAngle = Quaternion.Angle(transform.rotation, lookRotation);

        if(targetAngle <= 0.1f) {
            transform.rotation = lookRotation;
        }
    }

    public void Interact(GameObject interactor) {
        if(interactor.TryGetComponent(out PlayerController player)) {
            pauseMoveing = !pauseMoveing;

            targetPosition = player.transform.position;
        }
    }
}
