using UnityEngine;

public class AbilityUpgradeButton : MonoBehaviour
{
    Ability ability;
    PlayerAbilities abilities;
    public void Setup(Ability ability, PlayerAbilities abilities)
    {
        this.ability = ability;
        this.abilities= abilities;
    }
    public void Clicked()
    {
        abilities.UpgradeAbility(ability);
    }
}
