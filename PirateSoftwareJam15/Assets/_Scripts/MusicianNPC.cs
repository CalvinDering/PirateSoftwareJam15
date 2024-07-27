using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicianNPC : NPC, ILightable {

    [SerializeField] private ProblemType problemType;
    [SerializeField] private GameObject problemObjectPrefab;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private float cooldownTime = 10f;
    [SerializeField] private float despawnWaitingTime = 2f;
    [SerializeField] private float checkRadius = 20f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private bool lerpAnimations = true;

    private float problemValue;

    private float cooldownTimer;
    private bool isProblemSpawned = false;

    private GameObject[] problemObject;

    private void Awake() {
       problemObject = new GameObject[spawnPositions.Length];
    }

    private void Update() {
        if(isProblemSpawned) {
            if(lerpAnimations) {
                problemValue = Mathf.Lerp(problemValue, 1, Time.deltaTime);
            } else {
                problemValue = 1;
            }
        } else {
            if(lerpAnimations) {
                problemValue = Mathf.Lerp(problemValue, 0, Time.deltaTime);
            } else {
                problemValue = 0;
            }

            if(CheckForPlayer()) {
                cooldownTimer = cooldownTime * Random.Range(0.8f, 1.2f);
            }
            if(cooldownTimer <= 0) {
                SpawnProblem();
            } else {
                cooldownTimer -= Time.deltaTime;
            }
        }
        Debug.Log(problemValue);
        animator.SetFloat("problem", problemValue);
    }

    private void SpawnProblem() {
        for(int i = 0; i < spawnPositions.Length; i++) {
            problemObject[i] = Instantiate(problemObjectPrefab, spawnPositions[i]);
        }
        isProblemSpawned = true;
    }

    private IEnumerator DespawnProblemObject() {
        yield return new WaitForSeconds(despawnWaitingTime);
        for(int i = 0; i < spawnPositions.Length; i++) {
            Destroy(problemObject[i]);
        }
        cooldownTimer = cooldownTime;
        isProblemSpawned = false;
    }

    public void Lighten() {

        switch(problemType) {
            case ProblemType.alcohol:
                break;
            case ProblemType.calling:
                break;
            case ProblemType.groupies:
                break;
            case ProblemType.rehearsal:
                break;
            case ProblemType.smoking:
                break;
            default:
                break;
        }

        if(isProblemSpawned) {
            StartCoroutine(DespawnProblemObject());
        }
    }

    private bool CheckForPlayer() {
        if(Physics.CheckSphere(transform.position, checkRadius, playerLayerMask)) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    public enum ProblemType {
        alcohol,
        calling,
        groupies,
        rehearsal,
        smoking
    }
}
