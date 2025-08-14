using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationData : ScriptableObject {
    [SerializeField] private Vector2Int gridDimensions;
    [SerializeField] private int gridCellSize;
    [SerializeField] private MapCellData cellData;

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellData.GetCellsByOrientation(orientation);
    }

    public GameObject GetStartingCell() {
        return cellData.StartingCell;
    }

    public GameObject GetExitCell() {
        return cellData.ExitCell;
    }

    public GameObject GetDoor() {
        return cellData.Door;
    }

    public Vector2Int GridDimensions { get { return gridDimensions; } }

    public int GridCellSize { get { return gridCellSize; } }
}
