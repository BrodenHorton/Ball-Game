using UnityEngine;

public class InteractionManager : MonoBehaviour {
    public static InteractionManager Instance;

    [SerializeField] private InteractionUI interactionUI;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EventBus.OnInteractableEntered += UpdateUI;
        EventBus.OnInteractableExited += UpdateUI;
    }

    public void UpdateUI(PlayerInteraction playerInteraction) {
        if (playerInteraction.Interactions.Count <= 0) {
            interactionUI.gameObject.SetActive(false);
            return;
        }

        interactionUI.gameObject.SetActive(true);
        interactionUI.UpdateUI(playerInteraction);
    }
}