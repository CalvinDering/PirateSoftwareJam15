using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    [SerializeField] private CanvasGroup canvasGroup;
    private bool fadeIn = false;
    private bool fadeOut = false;

    public float timeToFade;

    private void Update() {
        if(fadeIn) {
            if(canvasGroup.alpha < 1) {
                canvasGroup.alpha += timeToFade * Time.deltaTime;
                if(canvasGroup.alpha >= 1) {
                    fadeIn = false;
                }
            }
        }

        if(fadeOut) {
            if(canvasGroup.alpha >= 0) {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;
                if(canvasGroup.alpha <= 0) {
                    fadeOut = false;
                }
            }
        }
    }

    public void FadeIn() {
        fadeIn = true;
    }

    public void FadeOut() {
        fadeOut = true;
    }

    public float GetTimeToFade() {
        return timeToFade;
    }
}
