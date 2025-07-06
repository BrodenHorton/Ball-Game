using UnityEngine;

public interface MapGenerator {
    GridCell[,] GenerateMap(Vector3 mapCenter, int mapSeed);
}
