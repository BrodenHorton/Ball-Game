using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    private List<Interactable> interactions;
    private int selectedInteractionIndex;

    private void Awake() {
        interactions = new List<Interactable>();
        selectedInteractionIndex = 0;
    }

    public void Add(Interactable interaction) {
        interactions.Add(interaction);
        EventBus.OnInteractableEntered?.Invoke(this);
    }

    public void Remove(Interactable interaction) {
        interactions.Remove(interaction);
        EventBus.OnInteractableExited?.Invoke(this);
    }

    public void PlayerInteract(InputAction.CallbackContext context) {
        if (!context.performed || interactions.Count <= 0)
            return;

        Mathf.Clamp(selectedInteractionIndex, 0, interactions.Count);
        interactions[selectedInteractionIndex].Interact();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Interactable>() == null)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        Add(interactable);
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Interactable>() == null)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        Remove(interactable);
    }

    public List<Interactable> Interactions => interactions;

    public int SelectedInteractionIndex => selectedInteractionIndex;
}
