using UnityEngine;

public class Map : MonoBehaviour {
    [SerializeField] private int mapSeed;
    [SerializeField] private Vector3 mapCenter;

    private GridCell[,] gridCells;
    private MapGenerator mapGenerator;

    private void Awake() {
        mapGenerator = GetComponent<MapGenerator>();
    }

    private void Start() {
        if (mapGenerator != null)
            gridCells = mapGenerator.GenerateMap(mapCenter, mapSeed);
        else
            Debug.Log("No MapGenerator script found on Map object.");
    }

    public int MapSeed => mapSeed;

    public Vector3 MapCenter => mapCenter;

    public GridCell[,] GridCells => gridCells;
}
