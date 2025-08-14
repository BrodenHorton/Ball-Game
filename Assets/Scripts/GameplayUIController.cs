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
    void AbilityUpdated(AbilityData abilityData, int index)
    {
        var icon = icons[index];
        icon.sprite = abilityData.Icon;
    }
    void AbilityAdded(AbilityData abilityData, int index)
    {
        var icon = Instantiate(iconPrefab, abilities);
        icons[index] = icon;
        icon.GetComponentInChildren<TextMeshProUGUI>().text = (index+1).ToString();
        icon.sprite = abilityData.Icon;
        abilities.gameObject.ToggleActiveIfChildrenExist();
    }
    void AbilityRemoved(AbilityData abilityData, int index) 
    {
        var icon = icons[index];
        icons[index] = null;
        Destroy(icon);
        abilities.gameObject.ToggleActiveIfChildrenExist();
    }
}
