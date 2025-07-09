using UnityEngine;

public class MeteorShower : Ability
{
    Transform player;
    Timer meteorSpawnRateTimer;
    MeteorData meteorData => abilityData as MeteorData;
    public override void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        activationTimer = new Timer(meteorData.activatedLength);
        meteorSpawnRateTimer = new Timer(meteorData.spawnRate);
        player = GameManager.Instance.getPlayer().transform;
        Debug.Log("Activating Meteor Shower");
    }

    public override void DashedIntoEventHandler(GameObject enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        isActivated = false;
        Debug.Log("Deactivating Meteor Shower");
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;

        Debug.Log("Meteor Shower Active");
        meteorSpawnRateTimer.Update();
        activationTimer.Update();
        if (activationTimer.IsFinished())
        {
            Deactivate();
            return;
        }
        if (meteorSpawnRateTimer.IsFinished())
        {
            var meteor = Instantiate(meteorData.meteorPrefab, player.position + (Vector3.up * meteorData.heightOffsetOfMeteor) + new Vector3(Random.Range(0, meteorData.spawnRadius), 0,0), Quaternion.identity);
            meteor.GetComponent<Rigidbody>().AddForce((Vector3.down + new Vector3(Random.Range(-meteorData.meteorDownwardMaxAngle, meteorData.meteorDownwardMaxAngle), 0, Random.Range(-meteorData.meteorDownwardMaxAngle, meteorData.meteorDownwardMaxAngle))).normalized * meteorData.meteorSpeed, ForceMode.Impulse);
            meteorSpawnRateTimer.Reset();
        }

    }
}
