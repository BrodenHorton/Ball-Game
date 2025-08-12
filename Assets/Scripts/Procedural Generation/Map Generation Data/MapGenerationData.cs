using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationData : ScriptableObject {
    [SerializeField] private Vector2Int gridDimensions;
    [SerializeField] private int gridCellSize;
    [SerializeField] private GameObject startingCell;
    [SerializeField] private GameObject exitCell;
    [SerializeField] private GameObject door;
    [SerializeField] private MapCellData cellData;

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellData.GetCellsByOrientation(orientation);
    }

    public Vector2Int GridDimensions => gridDimensions;

    public int GridCellSize => gridCellSize;

    public GameObject StartingCell => startingCell;

    public GameObject ExitCell => exitCell;

    public GameObject Door => door;
}
