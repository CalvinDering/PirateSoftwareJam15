using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable {

    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private float rotationAngle = 120f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float cooldownTimer = 8f;

    private float timer;

    private SoundFXPlayer audioPlayer;
    private Quaternion targetRotation;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Awake() {
        audioPlayer = GetComponent<SoundFXPlayer>();
    }

    private void Update() {

        if(!isMoving) {
            if(isOpen) {
                if(timer < 0) {
                    Interact(gameObject);
                } else {
                    timer -= Time.deltaTime;
                }
            }
            
            return;
        }

        if(Quaternion.Angle(transform.localRotation, targetRotation) <= 0.01f) {
            isMoving = false;
        } else {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

    }

    public void Interact(GameObject interactor) {
        if(isMoving) {
            return;
        }
        timer += cooldownTimer;
        targetRotation = Quaternion.Euler(0, (isOpen ? 0 : 1) * rotationAngle + rotationOffset, 0);

        isOpen = !isOpen;
        isMoving = true;
        if(isOpen) {
            audioPlayer.PlayRandomSoundWithoutTimer();
        } else {
            audioPlayer.PlayOtherRandomSoundWithoutTimer();
        }
    }
}
