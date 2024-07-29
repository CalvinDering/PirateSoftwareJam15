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

    [SerializeField] private float audioCooldown = 10f;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    private bool audioIsActive = false;

    private float audioTimer;

    private Room room;

    [HideInInspector] public float energy;
    private float problemValue;

    private float cooldownTimer;
    [HideInInspector] public bool isProblemSpawned = false;
    public bool nightStarted = false;

    private GameObject[] problemObject;

    private void Awake() {
        problemObject = new GameObject[spawnPositions.Length];
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(!nightStarted) {
            return;
        }

        if(isProblemSpawned) {
            if(lerpAnimations) {
                problemValue = Mathf.Lerp(problemValue, 1, Time.deltaTime);
            } else {
                problemValue = 1;
            }

            PlayRandomAudio();

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
            energy += Time.deltaTime;
        }
        animator.SetFloat("problem", problemValue);
    }

    public void StartNight() {
        energy = 0;
        nightStarted = true;
    }

    public void EndNight() {
        nightStarted = false;
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
        StopAudio();
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

    private void PlayRandomAudio() {

        if(audioTimer > 0) {
            audioTimer -= Time.deltaTime;
        } else {
            if(!audioIsActive) {
                if(!audioSource.isPlaying) {
                    audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                    audioSource.Play();
                    audioIsActive = true;
                }
            } else {
                if(!audioSource.isPlaying) {
                    audioTimer = audioCooldown;
                    audioIsActive = false;
                }
            }
        }
    }

    private void StopAudio() {
        audioSource.Stop();
    }

    public void SetRoom(Room room) {
        this.room = room;
    }

    public Room GetRoom() {
        return room;
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
