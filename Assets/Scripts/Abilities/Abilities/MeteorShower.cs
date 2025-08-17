using UnityEngine;

public class MeteorShower : Ability
{
    [SerializeField] private MeteorData abilityData;
    Transform player;
    Timer meteorSpawnRateTimer;

    public override void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        activationTimer = new Timer(abilityData.ActivatedLength, Deactivate);
        meteorSpawnRateTimer = new Timer(abilityData.spawnRate, SpawnMeteor);
        player = GameManager.Instance.getPlayer().transform;
        Debug.Log("Activating Meteor Shower");
    }

    public override void DashedIntoEventHandler(GameObject enemy)
    {
        
    }

    public override void Deactivate()
    {
        isActivated = false;
        Debug.Log("Deactivating Meteor Shower");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        Debug.Log("Meteor Shower Active");
        meteorSpawnRateTimer.Update();
        activationTimer.Update();
    }
    void SpawnMeteor()
    {
        var meteor = Instantiate(abilityData.meteorPrefab, player.position + (Vector3.up * abilityData.heightOffsetOfMeteor) + new Vector3(Random.Range(0, abilityData.spawnRadius), 0, 0), Quaternion.identity);
        meteor.GetComponent<Meteor>().Prepare(abilityData);
        Vector3 direction = Vector3.down.GetRandomDirectionWithinCone(abilityData.meteorDownwardMaxAngle);
        meteor.GetComponent<Rigidbody>().AddForce(direction * abilityData.meteorSpeed, ForceMode.Impulse);
        meteorSpawnRateTimer.Reset();
    }

    public MeteorData AbilityData { get { return abilityData; } set { abilityData = value; } }
}
