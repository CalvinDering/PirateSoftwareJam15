using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusician : MonoBehaviour {

    private Animator animator;

    [SerializeField] private float movementCooldown = 5f;

    private float timer;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        timer = movementCooldown;
    }

    private void Update() {
        if(timer < 0) {
            animator.SetTrigger("movement");
            timer += movementCooldown;
        } else {
            timer -= Time.deltaTime;
        }

    }

}
