using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class MapGenerator : MonoBehaviour {
    [SerializeField] protected bool hasDepthValueSprites;
    [SerializeField] protected GameObject depthValueSprite;

    public abstract Map GenerateMap(int seed);

    protected abstract void PlaceStartingCell(Map map);

    protected abstract void PlaceExitCell(Map map);

    public abstract void BuildMapCells(Map map, Transform parent);

    protected int NumberOfAdjacentCells(GridCell[,] gridCells, Vector2Int cell) {
        int count = 0;
        foreach (Direction2D direction in Enum.GetValues(typeof(Direction2D))) {
            Vector2Int adjacentCell = new Vector2Int(cell.x + direction.Vector().x, cell.y + direction.Vector().y);
            if (IsGridIndexInBounds(gridCells, adjacentCell) && gridCells[adjacentCell.y, adjacentCell.x] != null)
                count++;
        }

        return count;
    }

    protected bool IsGridIndexInBounds(GridCell[,] gridCells, Vector2Int cell) {
        return cell.x >= 0 && cell.x < gridCells.GetLength(1) && cell.y >= 0 && cell.y < gridCells.GetLength(0);
    }

    protected float getGridCellRotation(GridCell gridCell, Random rng) {
        CellOrientation orientation = gridCell.GetOrientation();
        float rotation = 0;
        if (orientation == CellOrientation.DeadEnd) {
            for (int i = 0; i < gridCell.walls.Length; i++) {
                if (!gridCell.walls[i]) {
                    rotation = i * 90;
                    break;
                }
            }
        }
        else if (orientation == CellOrientation.Corridor) {
            rotation = gridCell.walls[0] ? 90 : 0;
            rotation = rng.Next(0, 2) == 1 ? rotation + 180 : rotation;
        }
        else if (orientation == CellOrientation.Bend) {
            if (gridCell.walls[1] && gridCell.walls[2])
                rotation = 90;
            else if (gridCell.walls[2] && gridCell.walls[3])
                rotation = 180;
            else if (gridCell.walls[3] && gridCell.walls[0])
                rotation = 270;
        }
        else if (orientation == CellOrientation.Intersection) {
            rotation = rng.Next(0, 4) * 90;
        }
        else if (orientation == CellOrientation.T_Intersection) {
            for (int i = 0; i < gridCell.walls.Length; i++) {
                if (gridCell.walls[i]) {
                    rotation = i * 90;
                    break;
                }
            }
        }

        return rotation;
    }

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
