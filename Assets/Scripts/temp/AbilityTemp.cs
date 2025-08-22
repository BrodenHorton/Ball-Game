using UnityEngine;

[System.Serializable]
public abstract class AbilityTemp {
    public bool isPassive;
    public bool isActivated;
    protected Timer cooldownTimer;

    public AbilityTemp(bool isPassive) {
        this.isPassive = isPassive;
        isActivated = false;
    }

    public void Tick() {
        if (!isActivated || cooldownTimer == null)
            return;

        cooldownTimer.Update();
    }

    public abstract void Activate(GameObject parent);
}
