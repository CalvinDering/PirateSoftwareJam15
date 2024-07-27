using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {


    private List<MusicianNPC> musicians;

    [SerializeField] private PlayerController player;
    [SerializeField] private int mainMenuScene;
    [SerializeField] private float nightTime;
    [SerializeField] private GameObject startgameText;
    [SerializeField] private GameObject endgameStats;
    [SerializeField] private TextMeshProUGUI energyText;

    private float nightTimer;
    private bool nightStarted = false;

    private float energy;

    private void Awake() {
        musicians = FindObjectsOfType<MusicianNPC>().ToList();
        energy = 100;
        endgameStats.SetActive(false);
        player.gameStated = false;
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
        startgameText.SetActive(false);
        player.gameStated = true;
    }

    private void EndNight() {
        GetMusicianEnergy();
        float reachedEnergy = energy / musicians.Count * 100 / nightTime;
        nightStarted = false;
        int convertedEnergy = Mathf.Clamp((int) reachedEnergy, 0, 100);
        energyText.text = convertedEnergy.ToString() + "%";
        endgameStats.SetActive(true);
        player.gameStated = false;
    }

    public void Retry() {
        SceneManager.LoadScene(mainMenuScene);
    }

}
