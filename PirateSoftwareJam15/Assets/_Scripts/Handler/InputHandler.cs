using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour {

    public static InputHandler Instance;

    private InputActionAsset inputActionAsset;
    private InputActionMap playerActions;

    private InputAction movementInput;
    private InputAction lookInput;
    private InputAction interact;

    public Vector2 Movement;
    public Vector2 Look;

    private PlayerController playerController;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }

        playerController = GetComponent<PlayerController>();

        inputActionAsset = GetComponent<PlayerInput>().actions;
        playerActions = inputActionAsset.FindActionMap("Player");
        Movement = Vector2.zero;
        Look = Vector2.zero;
    }

    private void Update() {
        Movement = movementInput.ReadValue<Vector2>();
        Look = lookInput.ReadValue<Vector2>();
    }

    private void OnEnable() {
        movementInput = playerActions.FindAction("Move");
        lookInput = playerActions.FindAction("Look");
        interact = playerActions.FindAction("Interact");

        interact.performed += context => playerController.Interact();

        playerActions.Enable();
    }

    private void OnDisable() {
        playerActions.Disable();
    }

}
