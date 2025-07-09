using System;
using UnityEngine;

public class MapManager : MonoBehaviour {
    [SerializeField] bool hasRandomSeed;
    [SerializeField] int mapSeed;

    private Map map;
    private MapGenerator mapGenerator;

    private void Awake() {
        mapGenerator = GetComponent<MapGenerator>();
    }

    private void Start() {
        mapSeed = Guid.NewGuid().GetHashCode();

        if (mapGenerator != null) {
            if (hasRandomSeed)
            map = mapGenerator.GenerateMap(mapSeed, transform);
        }
        else
            Debug.Log("No MapGenerator script found on Map object.");
    }

    public Map Map => map;
}
