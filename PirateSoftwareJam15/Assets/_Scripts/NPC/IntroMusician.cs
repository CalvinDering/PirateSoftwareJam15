using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusician : MonoBehaviour {

    [SerializeField] private GameObject normalCharacter;
    [SerializeField] private GameObject zombieCharacter;
    [SerializeField] private GameObject extraObject;

    private Animator animator;

    [SerializeField] private float movementCooldown = 5f;

    private float timer;

    private void Awake() {
        DisplayAsZombie(false);
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

    public void DisplayAsZombie(bool zombie) {
        zombieCharacter.SetActive(zombie);
        normalCharacter.SetActive(!zombie);
        if(extraObject != null) {
            extraObject.SetActive(!zombie);
        }

        if(zombie) {
            animator = zombieCharacter.GetComponent<Animator>();
        } else {
            animator = normalCharacter.GetComponent<Animator>();
        }

        animator.SetBool("zombie", zombie);
    }

}
