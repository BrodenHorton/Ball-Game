using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator : MonoBehaviour {

    public abstract Map GenerateMap(int seed, Transform parent);

    protected void CreateDepthMap(Map map) {
        // x and y of the Vector3Int represent the cell coordinates and z represenets the cells depth value
        Queue<Vector3Int> cellQueue = new Queue<Vector3Int>();
        map.DepthByCell.Add(map.StartingCell, 0);
        cellQueue.Enqueue(new Vector3Int(map.StartingCell.x, map.StartingCell.y, 0));
        while(cellQueue.Count > 0) {
            Vector3Int pivotCell = cellQueue.Dequeue();
            if (!map.GridCells[pivotCell.y, pivotCell.x].walls[0] && pivotCell.y - 1 >= 0 && map.GridCells[pivotCell.y - 1, pivotCell.x] != null && !map.DepthByCell.ContainsKey(new Vector2Int(pivotCell.x, pivotCell.y - 1))) {
                Vector3Int cellDepthEntry = new Vector3Int(pivotCell.x, pivotCell.y - 1, pivotCell.z + 1);
                map.DepthByCell.Add(new Vector2Int(cellDepthEntry.x, cellDepthEntry.y), cellDepthEntry.z);
                cellQueue.Enqueue(cellDepthEntry);
            }
            if (!map.GridCells[pivotCell.y, pivotCell.x].walls[1] && pivotCell.x + 1 < map.GridCells.GetLength(0) && map.GridCells[pivotCell.y, pivotCell.x + 1] != null && !map.DepthByCell.ContainsKey(new Vector2Int(pivotCell.x + 1, pivotCell.y))) {
                Vector3Int cellDepthEntry = new Vector3Int(pivotCell.x + 1, pivotCell.y, pivotCell.z + 1);
                map.DepthByCell.Add(new Vector2Int(cellDepthEntry.x, cellDepthEntry.y), cellDepthEntry.z);
                cellQueue.Enqueue(cellDepthEntry);
            }
            if (!map.GridCells[pivotCell.y, pivotCell.x].walls[2] && pivotCell.y + 1 < map.GridCells.GetLength(1) && map.GridCells[pivotCell.y + 1, pivotCell.x] != null && !map.DepthByCell.ContainsKey(new Vector2Int(pivotCell.x, pivotCell.y + 1))) {
                Vector3Int cellDepthEntry = new Vector3Int(pivotCell.x, pivotCell.y + 1, pivotCell.z + 1);
                map.DepthByCell.Add(new Vector2Int(cellDepthEntry.x, cellDepthEntry.y), cellDepthEntry.z);
                cellQueue.Enqueue(cellDepthEntry);
            }
            if (!map.GridCells[pivotCell.y, pivotCell.x].walls[3] && pivotCell.x - 1 >= 0 && map.GridCells[pivotCell.y, pivotCell.x - 1] != null && !map.DepthByCell.ContainsKey(new Vector2Int(pivotCell.x - 1, pivotCell.y))) {
                Vector3Int cellDepthEntry = new Vector3Int(pivotCell.x - 1, pivotCell.y, pivotCell.z + 1);
                map.DepthByCell.Add(new Vector2Int(cellDepthEntry.x, cellDepthEntry.y), cellDepthEntry.z);
                cellQueue.Enqueue(cellDepthEntry);
            }
        }
    }
}
