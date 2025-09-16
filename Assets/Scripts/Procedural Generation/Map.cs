using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map {
    private GridCell[,] gridCells;
    private Vector3 mapOrigin;
    private float cellSize;
    private Vector2Int startingCell;
    private Vector2Int exitCell;
    private Dictionary<Vector2Int, int> depthByCell;
    private List<Transform> mobSpawnPositions;
    private List<Transform> lootSpawnPositions;
    private List<Transform> trapSpawnPositions;

    public Map(Vector2Int gridDimensions, Vector3 mapOrigin, float cellSize) {
        gridCells = new GridCell[gridDimensions.y, gridDimensions.x];
        this.mapOrigin = mapOrigin;
        this.cellSize = cellSize;
        depthByCell = new Dictionary<Vector2Int, int>();
        mobSpawnPositions = new List<Transform>();
        lootSpawnPositions = new List<Transform>();
        trapSpawnPositions = new List<Transform>();
    }

    public Vector3 GetMapStartingPosition() {
        return new Vector3(mapOrigin.x + startingCell.x * cellSize, 0f, mapOrigin.z - startingCell.y * cellSize);
    }

    public Vector3 GetMapExitPosition() {
        return new Vector3(mapOrigin.x + exitCell.x * cellSize, 0f, mapOrigin.z - exitCell.y * cellSize);
    }

    public List<Vector2Int> GetExistingCellIndices() {
        List<Vector2Int> cellIndices = new List<Vector2Int>();
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for (int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] != null)
                    cellIndices.Add(new Vector2Int(j, i));        
            }
        }

        return cellIndices;
    }

    public List<Vector2Int> GetCellIndicesOf(CellOrientation orientation) {
        List<Vector2Int> cellIndices = new List<Vector2Int>();
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for (int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] != null && gridCells[i, j].GetOrientation() == orientation)
                    cellIndices.Add(new Vector2Int(j, i));
            }
        }

        return cellIndices;
    }

    public GridCell[,] GridCells { get { return gridCells; } set { gridCells = value; } }

    public Vector3 MapOrigin { get { return mapOrigin; } }

    public float CellSize { get { return cellSize; } }

    public Vector2Int StartingCell { get { return startingCell; } set { startingCell = value; } }

    public Vector2Int ExitCell { get { return exitCell; } set { exitCell = value; } }

    public Dictionary<Vector2Int, int> DepthByCell { get { return depthByCell; } set { depthByCell = value; } }

    public List<Transform> MobSpawnPositions { get { return mobSpawnPositions; } set { mobSpawnPositions = value; } }

    public List<Transform> LootSpawnPositions { get { return lootSpawnPositions; } set { lootSpawnPositions = value; } }

    public List<Transform> TrapSpawnPositions { get { return trapSpawnPositions; } set { trapSpawnPositions = value; } }
}
