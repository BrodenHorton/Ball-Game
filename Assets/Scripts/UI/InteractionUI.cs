using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI interactionsMessage;

    private void Start() {
        gameObject.SetActive(false);
    }

    public void UpdateUI(PlayerInteraction playerInteraction) {
        interactionsMessage.text = "Alt: " + playerInteraction.Interactions[playerInteraction.SelectedInteractionIndex].GetMessage();
    }
}
