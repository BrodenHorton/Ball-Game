using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {
    private List<Transform> mobSpawnPositions;
    private List<Transform> lootSpawnPositions;

    private void Start() {
        mobSpawnPositions = new List<Transform>();
        lootSpawnPositions = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i) == null)
                continue;

            if (transform.GetChild(i).tag.Equals("MobSpawnPosition"))
                mobSpawnPositions.Add(transform.GetChild(i));
            if (transform.GetChild(i).tag.Equals("LootSpawnPosition"))
                lootSpawnPositions.Add(transform.GetChild(i));
        }

        Debug.Log("Mob spawn positions: " + mobSpawnPositions.Count);
        Debug.Log("Loot spawn positions: " + lootSpawnPositions.Count);
    }

}
