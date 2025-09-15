using UnityEngine;

public class Shrine : MonoBehaviour, Interactable
{
    [SerializeField]
    [TextArea(1, 2)] string interactMessage;
    public string GetMessage()
    {
        return interactMessage;
    }

    public void Interact()
    {
        Debug.Log("Interacting with Shrine");
        EventBus.OnOpenedAbilityUpgradeMenu?.Invoke();
    }
}
