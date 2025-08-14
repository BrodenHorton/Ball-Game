using System;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : MonoBehaviour {
    [SerializeField] bool hasRandomSeed;
    [SerializeField] int mapSeed;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private Rigidbody playerRb;

    private Map map;
    private MapGenerator mapGenerator;
    private MapLootGenerator lootGenerator;
    private MobGenerator mobGenerator;
    private bool isFirstUpdate;

    private void Awake() {
        mapGenerator = GetComponent<MapGenerator>();
        lootGenerator = GetComponent<MapLootGenerator>();
        mobGenerator = GetComponent<MobGenerator>();
        isFirstUpdate = true;
    }

    private void Start() {
        if (hasRandomSeed)
            mapSeed = Guid.NewGuid().GetHashCode();

        if (mapGenerator != null) {
            map = mapGenerator.GenerateMap(mapSeed);
            map.PrintGridCells();
            mapGenerator.BuildMapCells(map, transform);
        }
        else
            Debug.Log("No MapGenerator script found on Map object.");

        navMesh.BuildNavMesh();
        FindMobSpawnPositions();
        lootGenerator.GenerateLoot(map, mapSeed);
        mobGenerator.GenerateMobs(map, mapSeed);
    }

    private void FixedUpdate() {
        if(isFirstUpdate) {
            //playerRb.MovePosition(map.GetMapStartingPosition());
            isFirstUpdate = false;
        }
    }

    private void FindMobSpawnPositions() {
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag("MobSpawnPosition"))
                map.MobSpawnPositions.Add(child);
            if (child.CompareTag("LootSpawnPosition"))
                map.LootSpawnPositions.Add(child);
            if (child.CompareTag("TrapSpawnPosition"))
                map.TrapSpawnPositions.Add(child);
        }

        Debug.Log("Mob Spawning Positions: " + map.MobSpawnPositions.Count);
        Debug.Log("Loot Spawning Positions: " + map.LootSpawnPositions.Count);
        Debug.Log("Trap Spawning Positions: " + map.TrapSpawnPositions.Count);
    }

    public Map Map => map;
}
