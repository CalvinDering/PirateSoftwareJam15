using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI interactionTextfield;
    [SerializeField] private TextMeshProUGUI interactionTextfield2;
    [SerializeField] private string interactionText;

    private void Awake() {
        interactionTextfield.gameObject.SetActive(false);
    }

    public void ShowInteractionText() {
        ShowText(interactionText);
    }

    public void ShowText(string text) {
        interactionTextfield.text = text;
        interactionTextfield2.text = text;
        interactionTextfield.gameObject.SetActive(true);
    }

    public void Deactivate() {
        interactionTextfield.gameObject.SetActive(false);
    }

}
