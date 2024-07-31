using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour {

    private Rigidbody playerRB;
    private InputHandler inputHandler;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 75.0f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRange;
    [SerializeField] private Transform lightPoint;
    [SerializeField] private float lightRadius;
    [SerializeField] private float maxLightRange;
    [SerializeField] private float lightAngle;
    [SerializeField] private AudioClip[] audioClips;
    private int lastAudioIndex = -1;

    private AudioSource playerAudioSource;

    private float sensitivity;

    [HideInInspector] public bool gameStated = false;
    private bool pauseMoving = false;

    private PlayerUI playerUI;
    private IInteractable interactableObject;

    float rotationX = 0f;

    private void Awake() {
        playerRB = GetComponent<Rigidbody>();
        inputHandler = InputHandler.Instance;
        playerUI = GetComponent<PlayerUI>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(!gameStated) {
            return;
        }
        CheckLight();
        CheckInteract();
    }

    private void FixedUpdate() {
        if(!gameStated) {
            return;
        }
        Movement();
    }

    private void Movement() {
        if(!pauseMoving) {
            Vector3 move = cameraTransform.forward * inputHandler.Movement.y + cameraTransform.right * inputHandler.Movement.x;
            move.y = 0f;
            playerRB.AddForce(move.normalized * playerSpeed, ForceMode.VelocityChange);
        }

        rotationX += -inputHandler.Look.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * inputHandler.Look.x * lookSpeed * sensitivity);
    }

    public void SetSensitivity(float speed) {
        sensitivity = speed;
    }

    public float GetSensitivity() {
        return sensitivity;
    }

    public void CheckLight() {
        //RaycastHit[] hits = ConeCastAll(lightPoint.position, lightRadius, lightPoint.forward, maxLightRange, lightAngle);
        RaycastHit[] hits = Physics.SphereCastAll(lightPoint.position - new Vector3(0, 0, lightRadius), lightRadius, lightPoint.forward, maxLightRange);
        for(int i = 0; i < hits.Length; i++) {
            if(hits[i].collider.gameObject.TryGetComponent(out MusicianNPC musician)) {
                if(musician.GetRoom().IsInside(transform.position)) {
                    musician.Lighten();
                }
            }
            //hits.Where(c => c.collider.gameObject.TryGetComponent(out ILightable lightable)).ToList().ForEach(l => l.collider.GetComponent<ILightable>().Lighten());
        }
    }

    public void CheckInteract() {
        Ray ray = new Ray(interactionPoint.position, interactionPoint.forward);
        interactableObject = null;
        if(Physics.Raycast(ray, out RaycastHit hit, interactionRange)) {
            if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                playerUI.ShowInteractionText();
                interactableObject = interactable;
            }
        }
        if(interactableObject == null) {
            playerUI.Deactivate();
        }
    }

    public void Interact() {
        if(interactableObject == null) {
            return;
        }
        interactableObject.Interact(gameObject);
        Ray ray = new Ray(interactionPoint.position, interactionPoint.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactionRange)) {
            if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact(gameObject);

                if(hit.collider.gameObject.TryGetComponent(out NPC npc)) {
                    pauseMoving = !pauseMoving;
                }
            }
        }
    }

    public void PlayRandomAudio() {
        int randomIndex = Random.Range(0, audioClips.Length);
        if(randomIndex == lastAudioIndex) {
            randomIndex++;
            if(randomIndex >= audioClips.Length) {
                randomIndex = 0;
            }
        }
        lastAudioIndex = randomIndex;
        playerAudioSource.clip = audioClips[randomIndex];
        playerAudioSource.Play();
    }

    // Credits to: https://github.com/walterellisfun/ConeCast/tree/master
    private RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle) {
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        if(sphereCastHits.Length > 0) {
            for(int i = 0; i < sphereCastHits.Length; i++) {
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if(angleToHit < coneAngle) {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }
        }

        RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
        coneCastHits = coneCastHitList.ToArray();
        coneCastHits.OrderBy(h => h.transform.position - origin);
        return coneCastHits;

    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);

        Gizmos.DrawWireSphere(lightPoint.position + lightPoint.forward * maxLightRange, lightRadius);
    }
}
