using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {
    private List<Transform> mobSpawnPositions;
    private List<Transform> lootSpawnPositions;
    private List<Transform> trapSpawnPositions;

    private void Start() {
        mobSpawnPositions = new List<Transform>();
        lootSpawnPositions = new List<Transform>();
        trapSpawnPositions = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i) == null)
                continue;

            if (transform.GetChild(i).tag.Equals("MobSpawnPosition"))
                mobSpawnPositions.Add(transform.GetChild(i));
            if (transform.GetChild(i).tag.Equals("LootSpawnPosition"))
                lootSpawnPositions.Add(transform.GetChild(i));
            if (transform.GetChild(i).tag.Equals("TrapSpawnPosition"))
                trapSpawnPositions.Add(transform.GetChild(i));
        }

        Debug.Log("Mob spawn positions: " + mobSpawnPositions.Count);
        Debug.Log("Loot spawn positions: " + lootSpawnPositions.Count);
        Debug.Log("Trap spawn positions: " + trapSpawnPositions.Count);
    }

    public List<Transform> MobSpawnPositions => mobSpawnPositions;
    
    public List<Transform> LootSpawnPositions => lootSpawnPositions;

    public List<Transform> TrapSpawnPositions => trapSpawnPositions;

}
