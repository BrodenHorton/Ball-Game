using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Collider))]
public class ExitInteraction : MonoBehaviour, Interactable {
    private static readonly string INTERACTION_MESSAGE = "Escape the map!";

    public void Interact() {
        Debug.Log("You have interacted with the exit of the map!");
        SceneManager.LoadScene("Procedural Generation Dev Scene", LoadSceneMode.Single);
    }

    public string GetMessage() {
        return INTERACTION_MESSAGE;
    }
}
