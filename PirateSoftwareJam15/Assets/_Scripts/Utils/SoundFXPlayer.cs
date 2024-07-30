using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour {

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip[] audioClips2;
    [SerializeField] private float audioCooldown;
    [SerializeField] private bool updateManually = false;

    private AudioSource audioSource;

    private float audioTimer;
    private bool audioIsActive = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(updateManually) {
            return;
        }

        PlayRandomSound();
    }

    public void PlayRandomSound() {
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

    public void PlayRandomSoundWithoutTimer() {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        audioIsActive = true;
    }

    public void PlayOtherRandomSoundWithoutTimer() {
        audioSource.clip = audioClips2[Random.Range(0, audioClips2.Length)];
        audioSource.Play();
        audioIsActive = true;
    }

}
