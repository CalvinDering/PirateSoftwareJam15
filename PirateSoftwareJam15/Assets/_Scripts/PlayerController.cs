using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody playerRB;
    private InputHandler inputHandler;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 75.0f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRange;

    private bool pauseMoving = false;

    float rotationX = 0f;

    private void Awake() {
        playerRB = GetComponent<Rigidbody>();
        inputHandler = InputHandler.Instance;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if(!pauseMoving) {
            Vector3 move = cameraTransform.forward * inputHandler.Movement.y + cameraTransform.right * inputHandler.Movement.x;
            move.y = 0f;
            playerRB.AddForce(move.normalized * playerSpeed, ForceMode.VelocityChange);
        }

        rotationX += -inputHandler.Look.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * inputHandler.Look.x * lookSpeed);
    }

    public void Interact() {
        Ray ray = new Ray(interactionPoint.position, interactionPoint.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactionRange)) {
            if(hit.collider.gameObject.TryGetComponent(out ILightable lightable)) {
                lightable.Lighten();
            }
            /*if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact(gameObject);

                if(hit.collider.gameObject.TryGetComponent(out NPC npc)) {
                    pauseMoving = !pauseMoving;
                }
            }*/
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);
    }
}
