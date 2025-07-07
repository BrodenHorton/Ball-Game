using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationData : ScriptableObject {
    [SerializeField] protected Vector2Int gridDimensions;
    [SerializeField] protected int gridCellSize;
    [SerializeField] protected bool hasBranchPaths;
    [SerializeField] protected int randomWalkIterations;
    [SerializeField] protected int randomWalkLength;
    [SerializeField] protected int drunkWalkIterations;
    [SerializeField] protected int drunkWalkLength;
    [SerializeField] protected GameObject startingCell;
    [SerializeField] protected GameObject door;

    protected Dictionary<CellOrientation, List<GameObject>> cellsByOrientation;

    public MapGenerationData() {
        cellsByOrientation = new Dictionary<CellOrientation, List<GameObject>>();
        foreach (CellOrientation cellOrientation in Enum.GetValues(typeof(CellOrientation)))
            cellsByOrientation[cellOrientation] = new List<GameObject>();
    }

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellsByOrientation[orientation];
    }

    public Vector2Int GridDimensions => gridDimensions;

    public int GridCellSize => gridCellSize;
    
    public bool HasBranchPaths => hasBranchPaths;

    public int RandomWalkIterations => randomWalkIterations;

    public int MaxRandomWalkLength => randomWalkLength;

    public int DrunkWalkIterations => drunkWalkIterations;

    public int MaxDrunkWalkLength => drunkWalkLength;

    public GameObject StartingCell => startingCell;

    public GameObject Door => door;

    public Dictionary<CellOrientation, List<GameObject>> CellsByOrientation => cellsByOrientation;
}
