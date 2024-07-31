using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {


    private List<MusicianNPC> musicians;
    private List<IntroMusician> introMusicians;
    private Fade fade;

    [SerializeField] private MusicianSpawn[] musicianSpawns;
    [SerializeField] private PlayerController player;
    [SerializeField] private int mainMenuScene;
    [SerializeField] private float nightTime;
    [SerializeField] private GameObject startgameText;
    [SerializeField] private GameObject endgameStats;
    [SerializeField] private GameObject nightIsOverText;
    [SerializeField] private Transform playerSpawnpoint;
    [SerializeField] private Transform lobbySpawnpoint;
    [SerializeField] private TextMeshProUGUI energyText;

    private float nightTimer;
    private bool nightStarted = false;
    private bool gameEnded = false;

    private float energy;

    private void Awake() {
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
        player.transform.position = lobbySpawnpoint.position;
        player.transform.rotation = lobbySpawnpoint.rotation;
        player.gameStated = false;
        gameEnded = false;
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
        player.transform.position = playerSpawnpoint.position;
        player.transform.rotation = playerSpawnpoint.rotation;
        player.gameStated = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator EndNight() {
        player.gameStated = false;
        float fadeTime = fade.GetTimeToFade();

        fade.FadeIn();
        yield return new WaitForSeconds(fadeTime);
        player.transform.position = lobbySpawnpoint.position;
        player.transform.rotation = lobbySpawnpoint.rotation;
        CalcEnergy();

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
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry() {
        SceneManager.LoadScene(mainMenuScene);
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
