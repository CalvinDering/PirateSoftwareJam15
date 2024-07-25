using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicianNPC : NPC, ILightable {

    [SerializeField] private ProblemType problemType;
    [SerializeField] private GameObject problemObjectPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float cooldownTime = 10f;
    [SerializeField] private float despawnWaitingTime = 2f;
    [SerializeField] private float checkRadius = 20f;
    [SerializeField] private LayerMask playerLayerMask;

    private float cooldownTimer;
    private bool isProblemSpawned = false;

    private GameObject problemObject;

    private void Update() {
        if(isProblemSpawned) {
            return;
        }
        if(CheckForPlayer()) {
            cooldownTimer = cooldownTime * Random.Range(0.2f, 0.8f);
        }
        if(cooldownTimer <= 0) {
            SpawnProblem();
        } else {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void SpawnProblem() {
        problemObject = Instantiate(problemObjectPrefab, spawnPosition);
        isProblemSpawned = true;
    }

    private IEnumerator DespawnProblemObject() {
        yield return new WaitForSeconds(despawnWaitingTime);
        Destroy(problemObject);
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
