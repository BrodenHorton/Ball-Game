using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPickup : MonoBehaviour
{
    [SerializeField] Ability ability;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] Image icon;

    private void Awake()
    {
        abilityName.text = ability.GetAbilityData().name;
        icon.sprite = ability.GetAbilityData().icon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerAbilities>().AddAbility(ability))
        {
            print("Adding ability " + ability);
            Destroy(gameObject);
        }
    }

}
