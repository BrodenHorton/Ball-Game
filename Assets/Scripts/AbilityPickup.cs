using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPickup : MonoBehaviour
{
    [SerializeField] AbilityData abilityData;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] Image icon;

    private void Awake()
    {
        abilityName.text = abilityData.name;
        icon.sprite = abilityData.Icon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerAbilities>().AddAbility(abilityData))
        {
            print("Adding ability " + abilityData);
            Destroy(gameObject);
        }
    }

}
