using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomWalkMapGenerator : MapGenerator {
    [SerializeField] private RandomWalkMapGenerationData generationData;

    private Random rng;

    public override Map GenerateMap(int seed) {
        rng = new Random(seed);
        Vector3 mapOrigin = new Vector3(-generationData.GridDimensions.x * generationData.GridCellSize / 2, 0f, generationData.GridDimensions.y * generationData.GridCellSize / 2);
        Map map = new Map(generationData.GridDimensions, mapOrigin, generationData.GridCellSize);
        map.GridCells = new GridCell[generationData.GridDimensions.y, generationData.GridDimensions.x];
        Vector2Int originCell = new Vector2Int(generationData.GridDimensions.x / 2, generationData.GridDimensions.y / 2);
        
        RandomWalkGeneration(map.GridCells, originCell);
        if (generationData.HasBranchPaths)
            DrunkWalkBranchGeneration(map.GridCells);
        PlaceStartingCell(map);
        CreateDepthMap(map);
        PlaceExitCell(map);

        return map;
    }

    private void RandomWalkGeneration(GridCell[,] gridCells, Vector2Int originCell) {
        gridCells[originCell.y, originCell.x] = new GridCell();

        Vector2Int currentCell;
        for (int i = 0; i < generationData.RandomWalkIterations; i++) {
            currentCell = originCell;
            for (int j = 0; j < generationData.MaxRandomWalkLength; j++) {
                Array cardinalDirections = Enum.GetValues(typeof(Direction2D));
                Direction2D direction = (Direction2D)cardinalDirections.GetValue(rng.Next(cardinalDirections.Length));
                UpdateCellInDirectionOf(gridCells, ref currentCell, direction);
            }
        }
    }

    private void DrunkWalkBranchGeneration(GridCell[,] gridCells) {
        List<Vector2Int> activeGridCells = new List<Vector2Int>();
        for(int i = 0; i < gridCells.GetLength(0); i++) {
            for(int j = 0; j < gridCells.GetLength(1); j++) {
                if(gridCells[i, j] != null)
                    activeGridCells.Add(new Vector2Int(j, i));
            }
        }

        Vector2Int startingCell;
        Vector2 vectorDir;
        for(int i = 0; i < generationData.DrunkWalkIterations; i++) {
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
            for(int j = 0; j < generationData.MaxDrunkWalkLength; j++) {
                Direction2D direction = WeightedValues.GetWeightedValue(weightedDirections, rng);
                UpdateCellInDirectionOf(gridCells, ref currentCell, direction);
            }
        }
    }

    private void PlaceStartingCell(Map map) {
        for (int i = map.GridCells.GetLength(0) - 1; i >= 0; i--) {
            for (int j = 0; j < map.GridCells.GetLength(1); j++) {
                if (map.GridCells[i, j] != null) {
                    Vector2Int currentCell = new Vector2Int(j, i);
                    UpdateCellInDirectionOf(map.GridCells, ref currentCell, Direction2D.South);
                     map.StartingCell = currentCell;
                    return;
                }
            }
        }
    }

    private void PlaceExitCell(Map map) {
        int maxDepthValue = 0;
        foreach(KeyValuePair<Vector2Int, int> entry in map.DepthByCell) {
            if(entry.Value > maxDepthValue)
                maxDepthValue = entry.Value;
        }
        int exitDepthThreshold = maxDepthValue / 2;
        List<Vector2Int> exitRootCells = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, int> entry in map.DepthByCell) {
            if (entry.Value >= exitDepthThreshold)
                exitRootCells.Add(entry.Key);
        }

        exitRootCells.Shuffle(rng);

        bool isExitCellSet = false;
        foreach (Vector2Int exitRootCell in exitRootCells) {
            foreach (Direction2D direction in Enum.GetValues(typeof(Direction2D))) {
                if (!map.GridCells[exitRootCell.y, exitRootCell.x].walls[(int)direction])
                    continue;
                Vector2Int exitCell = new Vector2Int(exitRootCell.x + direction.Vector().x, exitRootCell.y + direction.Vector().y);
                if (!IsGridIndexInBounds(map.GridCells, exitCell))
                    continue;
                if (map.GridCells[exitCell.y, exitCell.x] != null)
                    continue;
                if (NumberOfAdjacentCells(map.GridCells, exitCell) != 1)
                    continue;

                map.GridCells[exitRootCell.y, exitRootCell.x].walls[(int)direction] = false;
                map.GridCells[exitCell.y, exitCell.x] = new GridCell();
                map.GridCells[exitCell.y, exitCell.x].walls[((int)direction + 2) % 4] = false;
                map.ExitCell = exitCell;
                map.DepthByCell.Add(exitCell, map.DepthByCell[exitRootCell] + 1);
                isExitCellSet = true;
                break;
            }

            if (isExitCellSet)
                break;
        }
    }

    private int NumberOfAdjacentCells(GridCell[,] gridCells, Vector2Int cell) {
        int count = 0;
        foreach(Direction2D direction in Enum.GetValues(typeof(Direction2D))) {
            Vector2Int adjacentCell = new Vector2Int(cell.x + direction.Vector().x, cell.y + direction.Vector().y);
            if (IsGridIndexInBounds(gridCells, adjacentCell) && gridCells[adjacentCell.y, adjacentCell.x] != null)
                count++;
        }

        return count;
    }

    private void UpdateCellInDirectionOf(GridCell[,] gridCells, ref Vector2Int currentCell, Direction2D direction) {
        Vector2Int nextCell = new Vector2Int(currentCell.x + direction.Vector().x, currentCell.y + direction.Vector().y);
        if (IsGridIndexInBounds(gridCells, nextCell)) {
            gridCells[currentCell.y, currentCell.x].walls[(int)direction] = false;
            if (gridCells[nextCell.y, nextCell.x] == null)
                gridCells[nextCell.y, nextCell.x] = new GridCell();
            gridCells[nextCell.y, nextCell.x].walls[((int)direction + 2) % 4] = false;
            currentCell = nextCell;
        }
    }

    private bool IsGridIndexInBounds(GridCell[,] gridCells, Vector2Int cell) {
        return cell.x >= 0 && cell.x < gridCells.GetLength(1) && cell.y >= 0 && cell.y < gridCells.GetLength(0);
    }

    public override void BuildMapCells(Map map, Transform parent) {
        for (int i = 0; i < map.GridCells.GetLength(0); i++) {
            for(int j = 0; j < map.GridCells.GetLength(1); j++) {
                if (map.GridCells[i, j] == null)
                    continue;

                CellOrientation orientation = map.GridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(map.GridCells[i, j]);
                GameObject cell;
                if (i == map.StartingCell.y && j == map.StartingCell.x)
                    cell = Instantiate(generationData.GetStartingCell(), parent);
                else if (i == map.ExitCell.y && j == map.ExitCell.x)
                    cell = Instantiate(generationData.GetExitCell(), parent);
                else
                    cell = Instantiate(generationData.GetCellsByOrientation(orientation)[0], parent);
                Vector3 cellCenter = new Vector3(j * generationData.GridCellSize + map.MapOrigin.x, map.MapOrigin.y, i * -generationData.GridCellSize + map.MapOrigin.z);
                cell.transform.localPosition = cellCenter;
                cell.transform.Rotate(cell.transform.up, rotation);

                if(generationData.GetDoor() != null) {
                    if (!map.GridCells[i, j].walls[1]) {
                        GameObject cellDoor = Instantiate(generationData.GetDoor(), parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x + generationData.GridDimensions.x / 2, cellCenter.y, cellCenter.z);
                        cellDoor.transform.Rotate(0f, 90f, 0f);
                    }
                    if (!map.GridCells[i, j].walls[2]) {
                        GameObject cellDoor = Instantiate(generationData.GetDoor(), parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x, cellCenter.y, cellCenter.z - generationData.GridDimensions.y / 2);
                    }
                }

                if(hasDepthValueSprites) {
                    GameObject cellDepthValue = Instantiate(depthValueSprite, parent);
                    cellDepthValue.transform.localPosition = cellCenter;
                    TextMesh textMesh = cellDepthValue.GetComponent<TextMesh>();
                    if(textMesh != null)
                        textMesh.text = map.DepthByCell[new Vector2Int(j, i)].ToString();
                }
            }
        }

        Debug.Log("Map construction finsihed");
    }

    private float getGridCellRotation(GridCell gridCell) {
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
            if(gridCell.walls[1] && gridCell.walls[2])
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

}
