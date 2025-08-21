using UnityEngine;

public class IgniteTest : AbilityGeneric<IgniteData>
{

    private void Start() {
        Debug.Log(abilityData.ActivatedLength);
    }

    public override void Activate() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}
