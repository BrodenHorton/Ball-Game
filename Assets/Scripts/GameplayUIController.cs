using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] RectTransform abilities;
    Image[] icons = new Image[3];
    [SerializeField] Image iconPrefab;
    [SerializeField] GameObject abilityUpgradeScreen;
    [SerializeField] AbilityUpgradeButton abilityUpgradeButton;
    [SerializeField] Transform abilityUpgradeScreenContainer;
    private void OnEnable()
    {
        EventBus.AbilityAdded += AbilityAdded;
        EventBus.AbilityRemoved += AbilityRemoved;
        EventBus.AbilityUpdated += AbilityUpdated;
        EventBus.OnClosedAbilityUpgradeMenu += CloseAbilityUpgrade;
        EventBus.OnOpenedAbilityUpgradeMenu += OpenAbilityUpgrade;
    }
    private void OnDisable()
    {
        EventBus.AbilityAdded -= AbilityAdded;
        EventBus.AbilityRemoved -= AbilityRemoved;
        EventBus.AbilityUpdated -= AbilityUpdated;
        EventBus.OnClosedAbilityUpgradeMenu -= CloseAbilityUpgrade;
        EventBus.OnOpenedAbilityUpgradeMenu -= OpenAbilityUpgrade;
    }
    private void Awake()
    {
        abilities.gameObject.ToggleActiveIfChildrenExist();
        abilityUpgradeScreen.SetActive(false);
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
    void CloseAbilityUpgrade()
    {
        abilityUpgradeScreenContainer.DestroyAllChildren();
        abilityUpgradeScreen.SetActive(false);
    }
    void OpenAbilityUpgrade()
    {
        abilityUpgradeScreen.SetActive(true);
        PlayerAbilities playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        foreach (Ability ability in playerAbilities.GetAbilities())
        {
            AbilityUpgradeButton upgradeButton = Instantiate(abilityUpgradeButton, abilityUpgradeScreenContainer);
            upgradeButton.Setup(ability, playerAbilities);
            var image = upgradeButton.GetComponent<Image>();
            var text = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
            image.sprite = ability.GetAbilityData().Icon;
            text.text = ability.name;
        }
    }
}
