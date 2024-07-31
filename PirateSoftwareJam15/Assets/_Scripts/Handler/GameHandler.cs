using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {


    private List<MusicianNPC> musicians;
    private List<IntroMusician> introMusicians;
    private Fade fade;

    [SerializeField] private MusicianSpawn[] musicianSpawns;
    [SerializeField] private PlayerController player;
    [SerializeField] private int mainMenuScene;
    [SerializeField] private float nightTime;
    [SerializeField] private GameObject startgameText;
    [SerializeField] private GameObject additionalText;
    [SerializeField] private GameObject endgameStats;
    [SerializeField] private GameObject mouseSensText;
    [SerializeField] private GameObject nightIsOverText;
    [SerializeField] private Transform playerSpawnpoint;
    [SerializeField] private Transform lobbySpawnpoint;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI energyText2;

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    [SerializeField] private TextMeshProUGUI sliderValueText2;

    [SerializeField] private ProgressBar progressBar;

    private float nightTimer;
    private bool nightStarted = false;
    private bool gameEnded = false;

    private float energy;

    private void Awake() {
        Restart();
        slider.onValueChanged.AddListener(delegate {
            ValueChangeCheck();
        });
        ValueChangeCheck();
    }

    private void Update() {
        if(gameEnded) {
            return;
        }

        if(!nightStarted) {
            return;
        }

        if(nightTimer <= 0) {
            StartCoroutine(EndNight());
            gameEnded = true;
        } else {
            nightTimer -= Time.deltaTime;
        }

        float nightProgress = nightTimer * 100 / nightTime;
        progressBar.SetFill(nightProgress);
    }


    private void GetMusicianEnergy() {
        foreach(MusicianNPC musician in musicians) {
            musician.EndNight();
            energy += musician.energy;
        }
    }

    public void StartNight() {
        StartCoroutine(StartGameRound());
    }

    public IEnumerator StartGameRound() {
        float fadeTime = fade.GetTimeToFade();
        fade.FadeIn();
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = playerSpawnpoint.position;
        player.GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        player.transform.rotation = playerSpawnpoint.rotation;

        startgameText.SetActive(false);
        mouseSensText.SetActive(false);
        additionalText.SetActive(false);
        progressBar.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(fadeTime);

        fade.FadeOut();
        yield return new WaitForSeconds(fadeTime);

        energy = 0;
        nightTimer = nightTime;
        nightStarted = true;
        musicians.ForEach(m => m.StartNight());
        player.gameStarted = true;
    }

    private IEnumerator EndNight() {
        player.gameStarted = false;
        float fadeTime = fade.GetTimeToFade();

        fade.FadeIn();
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = lobbySpawnpoint.position;
        player.GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        player.transform.rotation = lobbySpawnpoint.rotation;
        CalcEnergy();

        progressBar.gameObject.SetActive(false);
        nightIsOverText.SetActive(true);
        yield return new WaitForSeconds(fadeTime);

        fade.FadeOut();
        yield return new WaitForSeconds(fadeTime);

        ShowStats();
    }

    private void CalcEnergy() {
        GetMusicianEnergy();
        float reachedEnergy = energy / musicians.Count * 100 / nightTime;
        nightStarted = false;
        int convertedEnergy = Mathf.Clamp((int) reachedEnergy, 0, 100);
        energyText.text = convertedEnergy.ToString() + "%";
        energyText2.text = convertedEnergy.ToString() + "%";

        int energyPerMusician = 100 / musicians.Count;
        for(int m = 0; m < introMusicians.Count; m++) {
            if(convertedEnergy >= m * energyPerMusician) {
                introMusicians[m].DisplayAsZombie(false);
            } else {
                introMusicians[m].DisplayAsZombie(true);
            }
        }
    }

    private void ShowStats() {
        endgameStats.SetActive(true);
        mouseSensText.SetActive(true);
        additionalText.SetActive(true);
        progressBar.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry() {
        musicians.ForEach(m => Destroy(m.gameObject));

        Restart();
        StartNight();
    }

    public void Restart() {
        fade = GetComponent<Fade>();
        musicians = new List<MusicianNPC>();
        introMusicians = FindObjectsOfType<IntroMusician>().ToList();
        nightIsOverText.SetActive(false);
        for(int i = 0; i < musicianSpawns.Length; i++) {
            int spawnIndex = Random.Range(0, musicianSpawns[i].spawns.Length);
            MusicianNPC musician = Instantiate(musicianSpawns[i].musician, musicianSpawns[i].spawns[spawnIndex].spawnpoint);
            musician.SetRoom(musicianSpawns[i].spawns[spawnIndex].room);
            musicians.Add(musician);
        }
        energy = 100;
        endgameStats.SetActive(false);
        mouseSensText.SetActive(true);
        additionalText.SetActive(true);
        progressBar.gameObject.SetActive(false);
        player.transform.position = lobbySpawnpoint.position;
        player.GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(0, 0, 0);
        player.transform.rotation = lobbySpawnpoint.rotation;
        player.gameStarted = false;
        gameEnded = false;
    }

    private void ValueChangeCheck() {
        player.SetSensitivity(slider.value);
        sliderValueText.text = slider.value.ToString("F2");
        sliderValueText2.text = slider.value.ToString("F2");
    }

}

[System.Serializable]
public class MusicianSpawn {
    public Spawn[] spawns;
    public MusicianNPC musician;
}

[System.Serializable]
public class Spawn {
    public Room room;
    public Transform spawnpoint;
}

[System.Serializable]
public class Room {
    public Transform[] corners;

    public bool IsInside(Vector3 position) {
        if(position.x < corners[0].position.x || position.x > corners[1].position.x) {
            return false;
        }
        if(position.z < corners[0].position.z || position.z > corners[1].position.z) {
            return false;
        }
        return true;
    }
}
