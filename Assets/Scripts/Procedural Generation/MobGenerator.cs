using System;
using System.Collections.Generic;
using UnityEngine;

public class MobGenerator : MonoBehaviour {
    private static readonly int MIN_SPAWN_COUNT = 1;
    private static readonly int MAX_SPAWN_COUNT = 5;
    private static readonly float MAX_MOB_SPCING = 5f;

    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private int mobQuantity;

    private System.Random rng;

    public void GenerateMobs(Map map, int seed) {
        rng = new System.Random(seed);
        List<Transform> mobSpawnPositoins = new List<Transform>(map.MobSpawnPositions);
        mobSpawnPositoins.Shuffle(rng);
        int mobCount = 0;
        int positionIndex = 0;
        while (mobCount < mobQuantity && positionIndex < mobSpawnPositoins.Count) {
            int spawnCount = rng.Next(MIN_SPAWN_COUNT, MAX_SPAWN_COUNT + 1);
            if (spawnCount + mobCount > mobQuantity)
                spawnCount = mobQuantity - mobCount;
            for(int i = 0; i < spawnCount; i++) {
                Vector3 originSpawnPosition = mobSpawnPositoins[positionIndex].position;
                Vector3 mobPosition = new Vector3(
                    originSpawnPosition.x + (float)rng.NextDouble() * (MAX_MOB_SPCING * 2) - MAX_MOB_SPCING,
                    originSpawnPosition.y,
                    originSpawnPosition.z + (float)rng.NextDouble() * (MAX_MOB_SPCING * 2) - MAX_MOB_SPCING);
                GameObject mob = Instantiate(mobPrefab, mobPosition, mobSpawnPositoins[positionIndex].rotation);
            }

            mobCount += spawnCount;
            positionIndex++;
        }
    }
}
