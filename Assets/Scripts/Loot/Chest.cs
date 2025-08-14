using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Destructible {
    [SerializeField] private int playerLayer;
    [SerializeField] private float criticalImpactVelocity;
    [SerializeField] private LootTable lootTable;

    private void OnCollisionEnter(Collision collision) {
        if (ShouldBreak(collision.collider))
            Break();
    }

    public void Break() {
        //List<GameObject> loot = WeightedValues.GetWeightedValues(lootTable.WeightedLoot, 2);
        //foreach (GameObject item in loot)
        //    Instantiate(item);
        Destroy(gameObject);
    }

    public bool ShouldBreak(Collider collider) {
        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            return false;

        return collider.gameObject.layer == playerLayer && rb.linearVelocity.magnitude >= criticalImpactVelocity;
    }
}
