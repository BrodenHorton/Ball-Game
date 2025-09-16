using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomWalkMapGenerator : MapGenerator {
    [SerializeField] private RandomWalkMapGenerationData generationData;

    private Random rng;

    public override Map GenerateMap(MapCellData cellData, int seed) {
        rng = new Random(seed);
        Vector3 mapOrigin = new Vector3(-cellData.GridDimensions.x * cellData.GridCellSize / 2, 0f, cellData.GridDimensions.y * cellData.GridCellSize / 2);
        Map map = new Map(cellData.GridDimensions, mapOrigin, cellData.GridCellSize);
        map.GridCells = new GridCell[cellData.GridDimensions.y, cellData.GridDimensions.x];
        Vector2Int originCell = new Vector2Int(cellData.GridDimensions.x / 2, cellData.GridDimensions.y / 2);
        
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
            WeightedList<Direction2D> weightedDirections = new WeightedList<Direction2D>();
            if(vectorDir.y > 0) {
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.North, (int)(Math.Abs(vectorDir.y) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 14)));
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.South, 3));
            }
            else {
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.South, (int)(Math.Abs(vectorDir.y) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 14)));
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.North, 3));
            }

            if(vectorDir.x > 0) {
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.East, (int)(Math.Abs(vectorDir.x) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 14)));
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.West, 3));
            }
            else {
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.West, (int)(Math.Abs(vectorDir.x) / (Math.Abs(vectorDir.x) + Math.Abs(vectorDir.y)) * 14)));
                weightedDirections.WeightedEntries.Add(new WeightedEntry<Direction2D>(Direction2D.East, 3));
            }

            Vector2Int currentCell = startingCell;
            for(int j = 0; j < generationData.MaxDrunkWalkLength; j++) {
                Direction2D direction = weightedDirections.GetWeightedValue();
                UpdateCellInDirectionOf(gridCells, ref currentCell, direction);
            }
        }
    }

    protected override void PlaceStartingCell(Map map) {
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

    protected override void PlaceExitCell(Map map) {
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
}
