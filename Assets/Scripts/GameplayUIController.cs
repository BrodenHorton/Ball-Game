using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] RectTransform abilities;
    Image[] icons = new Image[3];
    [SerializeField] Image iconPrefab;
    private void OnEnable()
    {
        EventBus.AbilityAdded += AbilityAdded;
        EventBus.AbilityRemoved += AbilityRemoved;
        EventBus.AbilityUpdated += AbilityUpdated;
    }
    private void OnDisable()
    {
        EventBus.AbilityAdded -= AbilityAdded;
        EventBus.AbilityRemoved -= AbilityRemoved;
        EventBus.AbilityUpdated -= AbilityUpdated;
    }
    private void Awake()
    {
        abilities.gameObject.ToggleActiveIfChildrenExist();
    }
    void AbilityUpdated(Ability ability, int index)
    {
        var icon = icons[index];
        icon.sprite = ability.GetAbilityData().icon;
    }
    void AbilityAdded(Ability ability, int index)
    {
        var icon = Instantiate(iconPrefab, abilities);
        icons[index] = icon;
        icon.GetComponentInChildren<TextMeshProUGUI>().text = (index+1).ToString();
        icon.sprite = ability.GetAbilityData().icon;
        abilities.gameObject.ToggleActiveIfChildrenExist();
    }
    void AbilityRemoved(Ability ability, int index) 
    {
        var icon = icons[index];
        icons[index] = null;
        Destroy(icon);
        abilities.gameObject.ToggleActiveIfChildrenExist();
    }
}
