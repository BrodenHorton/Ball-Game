using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = System.Random;

public class Chest : MonoBehaviour, Destructible {
    [SerializeField] private string playerTag;
    [SerializeField] private float criticalImpactVelocity;
    [SerializeField] private int minLootAmt;
    [SerializeField] private int maxLootAmt;
    [SerializeField] private LootTable lootTable;
    [SerializeField] private float minDropDistance;
    [SerializeField] private float maxDropDistance;
    [SerializeField] private GameObject itemSpritePrefab;

    // Temp until there is a class that holds seeds
    private Random rng = new Random();

    public void Break() {
        List<GameObject> loot = lootTable.WeightedLoot.GetWeightedValues(rng.Next(minLootAmt, maxLootAmt + 1));
        foreach (GameObject item in loot) {
            Vector3 startingPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            Vector2 dropDirection = new Vector2((float)rng.NextDouble() * 2f - 1, (float)rng.NextDouble() * 2f - 1).normalized;
            float dropDistance = (float)rng.NextDouble() * (maxDropDistance - minDropDistance) + minDropDistance;
            Vector2 dropVector = dropDirection * dropDistance;
            Vector3 dropLocation = new Vector3(transform.position.x + dropVector.x, transform.position.y, transform.position.z + dropVector.y);
            ItemSprite itemSprite = Instantiate(itemSpritePrefab, transform.position, Quaternion.identity).GetComponent<ItemSprite>();
            itemSprite.Activate(startingPosition, dropLocation, item);
        }

        Destroy(gameObject);
    }

    public bool ShouldBreak(Collider collider) {
        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            return false;

        return collider.CompareTag(playerTag) && rb.linearVelocity.magnitude >= criticalImpactVelocity;
    }

    private void OnCollisionEnter(Collision collision) {
        if (ShouldBreak(collision.collider))
            Break();
    }
}
