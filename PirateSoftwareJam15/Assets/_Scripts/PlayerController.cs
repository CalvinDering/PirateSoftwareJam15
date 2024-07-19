using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody playerRB;
    private InputHandler inputHandler;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float playerSpeed;

    private void Awake() {
        playerRB = GetComponent<Rigidbody>();
        inputHandler = InputHandler.Instance;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        Vector3 move = cameraTransform.forward * inputHandler.Movement.y + cameraTransform.right * inputHandler.Movement.x;
        move.y = 0f;
        playerRB.AddForce(move.normalized * playerSpeed, ForceMode.VelocityChange);
    }

    public void Interact() {
    
    }
}
