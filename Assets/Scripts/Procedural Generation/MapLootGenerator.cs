using System.Collections.Generic;
using UnityEngine;

public class MapLootGenerator : MonoBehaviour {
    [SerializeField] private int lootQuantity;
    [SerializeField] private GameObject lootPrefab;

    private System.Random rng;

    public void GenerateLoot(Map map, int seed) {
        rng = new System.Random(seed);
        List<Transform> lootPositions = new List<Transform>(map.LootSpawnPositions);
        lootPositions.Shuffle(rng);
        for (int i = 0; i < lootQuantity && i < lootPositions.Count; i++) {
            GameObject loot = Instantiate(lootPrefab);
            loot.transform.position = lootPositions[i].transform.position;
            loot.transform.rotation = lootPositions[i].transform.rotation;
        }
    }
}
