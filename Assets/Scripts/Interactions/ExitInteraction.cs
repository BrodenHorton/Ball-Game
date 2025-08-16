using UnityEngine;

[RequireComponent (typeof(Collider))]
public class ExitInteraction : MonoBehaviour, Interactable {
    private static readonly string INTERACTION_MESSAGE = "Escape the map!";

    public void Interact() {
        Debug.Log("You have interacted with the exit of the map!");
    }

    public string GetMessage() {
        return INTERACTION_MESSAGE;
    }
}
