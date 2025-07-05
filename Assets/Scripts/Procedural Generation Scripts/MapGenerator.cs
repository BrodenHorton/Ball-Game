using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int gridDimensions;
    [SerializeField] private int gridCellSize;
    [SerializeField] private MapType mapType;
    [SerializeField] private bool hasBranchPaths;
    [SerializeField] private int mapSeed;
    [SerializeField] private Vector3 mapCenter;

    private GridCell[,] gridCells;
    private System.Random rng;

    private void Start() {
        gridCells = new GridCell[gridDimensions.y, gridDimensions.x];
        rng = new System.Random(mapSeed);

        GenerateGridCells();
        BuildMapCells();
        PrintGridCells();
    }

    private void GenerateGridCells() {
        Vector2Int startingCell = new Vector2Int(gridDimensions.x / 2, gridDimensions.y / 2);
        RandomWalkGeneration(startingCell);
        if(hasBranchPaths)
            DrunkWalkBranchGeneration();
    }

    private void RandomWalkGeneration(Vector2Int startingCell) {
        gridCells[startingCell.y, startingCell.x] = new GridCell();

        Vector2Int currentCell;
        for (int i = 0; i < mapType.RandomWalkIterations; i++) {
            currentCell = startingCell;
            for (int j = 0; j < mapType.MaxRandomWalkLength; j++) {
                Array cardinalDirections = Enum.GetValues(typeof(Direction2D));
                Direction2D direction = (Direction2D)cardinalDirections.GetValue(rng.Next(cardinalDirections.Length));
                UpdateCellInDirectionOf(ref currentCell, direction);
            }
        }
    }

    private void DrunkWalkBranchGeneration() {
        List<Vector2Int> activeGridCells = new List<Vector2Int>();
        for(int i = 0; i < gridCells.GetLength(0); i++) {
            for(int j = 0; j < gridCells.GetLength(1); j++) {
                if(gridCells[i, j] != null)
                    activeGridCells.Add(new Vector2Int(j, i));
            }
        }

        Vector2Int startingCell;
        Vector2 vectorDir;
        for(int i = 0; i < mapType.DrunkWalkIterations; i++) {
            startingCell = activeGridCells[rng.Next(0, activeGridCells.Count)];
            vectorDir = new Vector2((float)rng.NextDouble() * 2 - 1, (float)rng.NextDouble() * 2 - 1);
            List<WeightedEntry<Direction2D>> weightedDirections = new List<WeightedEntry<Direction2D>>();
            if(vectorDir.y > 0) {
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.North, Math.Abs(vectorDir.y) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 0.7f));
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.South, 0.15f));
            }
            else {
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.South, Math.Abs(vectorDir.y) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 0.7f));
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.North, 0.15f));
            }

            if(vectorDir.x > 0) {
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.East, Math.Abs(vectorDir.x) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 0.7f));
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.West, 0.15f));
            }
            else {
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.West, Math.Abs(vectorDir.x) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 0.7f));
                weightedDirections.Add(new WeightedEntry<Direction2D>(Direction2D.East, 0.15f));
            }

            Vector2Int currentCell = startingCell;
            for(int j = 0; j < mapType.MaxDrunkWalkLength; j++) {
                Direction2D direction = WeightedValues.GetWeightedValue(weightedDirections, rng);
                UpdateCellInDirectionOf(ref currentCell, direction);
            }
        }

    }

    private void UpdateCellInDirectionOf(ref Vector2Int currentCell, Direction2D direction) {
        if (direction == Direction2D.North && currentCell.y - 1 >= 0) {
            gridCells[currentCell.y, currentCell.x].walls[0] = false;
            if (gridCells[currentCell.y - 1, currentCell.x] == null)
                gridCells[currentCell.y - 1, currentCell.x] = new GridCell();
            gridCells[currentCell.y - 1, currentCell.x].walls[2] = false;
            currentCell.y--;
        }
        else if (direction == Direction2D.East && currentCell.x + 1 < gridCells.GetLength(1)) {
            gridCells[currentCell.y, currentCell.x].walls[1] = false;
            if (gridCells[currentCell.y, currentCell.x + 1] == null)
                gridCells[currentCell.y, currentCell.x + 1] = new GridCell();
            gridCells[currentCell.y, currentCell.x + 1].walls[3] = false;
            currentCell.x++;
        }
        else if (direction == Direction2D.South && currentCell.y + 1 < gridCells.GetLength(0)) {
            gridCells[currentCell.y, currentCell.x].walls[2] = false;
            if (gridCells[currentCell.y + 1, currentCell.x] == null)
                gridCells[currentCell.y + 1, currentCell.x] = new GridCell();
            gridCells[currentCell.y + 1, currentCell.x].walls[0] = false;
            currentCell.y++;
        }
        else if (direction == Direction2D.West && currentCell.x - 1 >= 0) {
            gridCells[currentCell.y, currentCell.x].walls[3] = false;
            if (gridCells[currentCell.y, currentCell.x - 1] == null)
                gridCells[currentCell.y, currentCell.x - 1] = new GridCell();
            gridCells[currentCell.y, currentCell.x - 1].walls[1] = false;
            currentCell.x--;
        }
    }

    private void BuildMapCells() {
        Vector3 mapOffset = new Vector3(-gridDimensions.x * gridCellSize / 2 + mapCenter.x, mapCenter.y, gridDimensions.y * gridCellSize / 2 + mapCenter.z);
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for(int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] == null)
                    continue;

                CellOrientation orientation = gridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(gridCells[i, j]);
                GameObject cell = Instantiate(mapType.GetCellsByOrientation(orientation)[0]);
                cell.transform.position = new Vector3(j * gridCellSize + mapOffset.x, mapCenter.y, i * -gridCellSize + mapOffset.z);
                cell.transform.Rotate(cell.transform.up, rotation);
            }
        }

        Debug.Log("Map construction finsihed");
    }

    private float getGridCellRotation(GridCell gridCell) {
        System.Random rand = new System.Random();
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
            rotation = rand.Next(0, 2) == 1 ? rotation + 180 : rotation;
        }
        else if (orientation == CellOrientation.Bend) {
            if(gridCell.walls[1] && gridCell.walls[2])
                rotation = 90;
            else if (gridCell.walls[2] && gridCell.walls[3])
                rotation = 180;
            else if (gridCell.walls[3] && gridCell.walls[0])
                rotation = 270;
        }
        else if (orientation == CellOrientation.Intersection) {
            rotation = rand.Next(0, 4) * 90;
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

    private void PrintGridCells() {
        string mapStr = "";
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for (int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] == null) {
                    mapStr = mapStr + "-";
                    continue;
                }

                CellOrientation orientation = gridCells[i, j].GetOrientation();
                if (orientation == CellOrientation.DeadEnd)
                    mapStr += "D";
                else if (orientation == CellOrientation.Corridor)
                    mapStr += "C";
                else if (orientation == CellOrientation.Bend)
                    mapStr += "B";
                else if (orientation == CellOrientation.T_Intersection)
                    mapStr += "T";
                else if (orientation == CellOrientation.Intersection)
                    mapStr += "I";
            }
            mapStr += "\n";
        }

        Debug.Log(mapStr);
    }

}
