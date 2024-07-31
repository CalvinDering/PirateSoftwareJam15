using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    [SerializeField] private int max;
    [SerializeField] private int current;

    [SerializeField] private Image mask;

    public void SetFill(float amount) {
        current = (int) Mathf.Clamp(amount, 0, max);
        float fillAmount = (float) current / (float) max;
        mask.fillAmount = fillAmount;
    }

    private void GetCurrentFill() {
        float fillAmount = (float) current / (float) max;
        mask.fillAmount = fillAmount;
    }

}
