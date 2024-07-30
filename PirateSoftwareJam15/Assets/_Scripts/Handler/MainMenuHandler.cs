using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {

    [SerializeField] private int gameSceneIndex;

    public void StartGame() {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
