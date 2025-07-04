using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapType : ScriptableObject {
    [SerializeField] protected int randomWalkIterations;
    [SerializeField] protected int minRandomWalkLength;
    [SerializeField] protected int maxRandomWalkLength;
    [SerializeField] protected int drunkWalkIterations;
    [SerializeField] protected int minDrunkWalkLength;
    [SerializeField] protected int maxDrunkWalkLength;

    protected Dictionary<CellOrientation, List<GameObject>> cellsByOrientation;

    public MapType() {
        cellsByOrientation = new Dictionary<CellOrientation, List<GameObject>>();
        foreach (CellOrientation cellOrientation in Enum.GetValues(typeof(CellOrientation)))
            cellsByOrientation[cellOrientation] = new List<GameObject>();
    }

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellsByOrientation[orientation];
    }

    public int RandomWalkIterations => randomWalkIterations;

    public int MinRandomWalkLength => minRandomWalkLength;

    public int MaxRandomWalkLength => maxRandomWalkLength;

    public int DrunkWalkIterations => drunkWalkIterations;

    public int MinDrunkWalkLength => minDrunkWalkLength;

    public int MaxDrunkWalkLength => maxDrunkWalkLength;

    public Dictionary<CellOrientation, List<GameObject>> CellsByOrientation => cellsByOrientation;
}
