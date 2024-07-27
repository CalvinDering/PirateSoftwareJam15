using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour {


    private List<MusicianNPC> musicians;

    [SerializeField] private float nightTime;
    private float nightTimer;
    private bool nightStarted = false;

    private float energy;

    private void Awake() {
        musicians = FindObjectsOfType<MusicianNPC>().ToList();
        energy = 100;
    }

    private void Update() {
        if(!nightStarted) {
            return;
        }

        if(nightTimer <= 0) {
            EndNight();
        } else {
            nightTimer -= Time.deltaTime;
        }
    }

    private void GetMusicianEnergy() {
        foreach(MusicianNPC musician in musicians) {
            musician.EndNight();
            energy += musician.energy;
        }
    }

    public void StartNight() {
        energy = 0;
        nightTimer = nightTime;
        nightStarted = true;
        musicians.ForEach(m => m.StartNight());
    }

    private void EndNight() {
        GetMusicianEnergy();
        float reachedEnergy = energy / musicians.Count * 100 / nightTime;
        Debug.Log("Night finished with Energy at " + reachedEnergy);
        nightStarted = false;
    }

}
