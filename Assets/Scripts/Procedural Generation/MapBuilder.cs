using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapBuilder : MonoBehaviour {
    [SerializeField] private int minPoiCount;
    [SerializeField] private int maxPoiCount;
    [SerializeField] private bool hasDepthValueSprites;
    [SerializeField] private GameObject depthValueSprite;

    private Random rng;

    public void BuildMapCells(Map map, MapCellData cellData, Transform parent, int seed) {
        rng = new Random(seed);
        List<Vector2Int> uniquePoiExcludedIndices = new List<Vector2Int>() { map.StartingCell, map.ExitCell };
        Dictionary<Vector2Int, GameObject> uniquePoiCellsByIndex = GenerateUniquePoisByIndex(map, cellData, uniquePoiExcludedIndices, rng);

        List<Vector2Int> poiExcludedIndices = new List<Vector2Int>() { map.StartingCell, map.ExitCell }.Concat(uniquePoiCellsByIndex.Keys).ToList();
        List<Vector2Int> poiIndices = GeneratePoiIndices(map, poiExcludedIndices, rng);

        int tempPoiCount = 0;
        for (int i = 0; i < map.GridCells.GetLength(0); i++) {
            for (int j = 0; j < map.GridCells.GetLength(1); j++) {
                if (map.GridCells[i, j] == null)
                    continue;

                CellOrientation orientation = map.GridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(map.GridCells[i, j], rng);
                GameObject cell;
                if (i == map.StartingCell.y && j == map.StartingCell.x)
                    cell = Instantiate(cellData.StartingCell, parent);
                else if (i == map.ExitCell.y && j == map.ExitCell.x)
                    cell = Instantiate(cellData.ExitCell, parent);
                else {
                    Vector2Int cellIndex = new Vector2Int(j, i);
                    bool isPoi = poiIndices.Contains(cellIndex);
                    GameObject cellPrefab = isPoi ?
                        cellData.GetPoiCellsByOrientation(orientation).GetWeightedValue()
                        : cellData.GetCellsByOrientation(orientation).GetWeightedValue();
                    cell = Instantiate(cellPrefab, parent);
                    if (isPoi)
                        tempPoiCount++;
                }
                Vector3 cellCenter = new Vector3(j * cellData.GridCellSize + map.MapOrigin.x, map.MapOrigin.y, i * -cellData.GridCellSize + map.MapOrigin.z);
                cell.transform.localPosition = cellCenter;
                cell.transform.Rotate(cell.transform.up, rotation);

                if (cellData.Door != null) {
                    if (!map.GridCells[i, j].walls[1]) {
                        GameObject cellDoor = Instantiate(cellData.Door, parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x + cellData.GridDimensions.x / 2, cellCenter.y, cellCenter.z);
                        cellDoor.transform.Rotate(0f, 90f, 0f);
                    }
                    if (!map.GridCells[i, j].walls[2]) {
                        GameObject cellDoor = Instantiate(cellData.Door, parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x, cellCenter.y, cellCenter.z - cellData.GridDimensions.y / 2);
                    }
                }

                if (hasDepthValueSprites) {
                    GameObject cellDepthValue = Instantiate(depthValueSprite, parent);
                    cellDepthValue.transform.localPosition = cellCenter;
                    TextMesh textMesh = cellDepthValue.GetComponent<TextMesh>();
                    if (textMesh != null)
                        textMesh.text = map.DepthByCell[new Vector2Int(j, i)].ToString();
                }
            }
        }

        Debug.Log("POIs generated: " + tempPoiCount);
    }

    // TODO: Fix Unique Pois not being added to list and returned.
    private Dictionary<Vector2Int, GameObject> GenerateUniquePoisByIndex(Map map, MapCellData cellData, List<Vector2Int> excludedCellIndices, Random rng) {
        Dictionary<Vector2Int, GameObject> uniquePoiCellsByIndex = new Dictionary<Vector2Int, GameObject>();
        foreach (CellOrientation orientation in Enum.GetValues(typeof(CellOrientation))) {
            List<GameObject> uniquePoiCells = cellData.GetUniquePoiCellsByOrientation(orientation);
            if (uniquePoiCells == null || uniquePoiCells.Count <= 0)
                continue;

            List<Vector2Int> existingCellIndices = map.GetCellIndicesOf(orientation);
            existingCellIndices.Shuffle(rng);
            while (existingCellIndices.Count > 0 && uniquePoiCells.Count > 0) {
                if (excludedCellIndices.Contains(existingCellIndices[0])) {
                    existingCellIndices.RemoveAt(0);
                    continue;
                }

                uniquePoiCellsByIndex.Add(existingCellIndices[0], uniquePoiCells[0]);
                existingCellIndices.RemoveAt(0);
                uniquePoiCells.RemoveAt(0);
            }

            if (uniquePoiCells.Count > 0)
                Debug.LogWarning("Not all Unique Cells could be placed. Missing " + orientation.ToString() + " Unique cell count: " + uniquePoiCells.Count);
        }

        Debug.Log("Number of unique cells being added to the map: " + uniquePoiCellsByIndex.Count);
        return uniquePoiCellsByIndex;
    }

    private List<Vector2Int> GeneratePoiIndices(Map map, List<Vector2Int> excludedCellIndices, Random rng) {
        List<Vector2Int> poiIndices = new List<Vector2Int>();
        int poiCount = rng.Next(minPoiCount, maxPoiCount + 1);
        List<Vector2Int> existingCellIndices = map.GetExistingCellIndices();
        existingCellIndices.Shuffle(rng);
        int count = 0;
        while (count < poiCount && existingCellIndices.Count > 0) {
            if (excludedCellIndices.Contains(existingCellIndices[0])) {
                existingCellIndices.RemoveAt(0);
                continue;
            }

            poiIndices.Add(existingCellIndices[0]);
            existingCellIndices.RemoveAt(0);
            count++;
        }

        return poiIndices;
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

    public int MinPoiCount { get { return minPoiCount; } }

    public int MaxPoiCount { get { return maxPoiCount; } }
}