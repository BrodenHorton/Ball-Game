using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapCellData", menuName = "Scriptable Objects/Map Cell Data")]
public class MapCellData : ScriptableObject {
    [SerializeField] private List<GameObject> deadEndCells;
    [SerializeField] private List<GameObject> bendCells;
    [SerializeField] private List<GameObject> corridorCells;
    [SerializeField] private List<GameObject> tIntersectionCells;
    [SerializeField] private List<GameObject> intersectionCells;
    [SerializeField] private GameObject startingCell;
    [SerializeField] private GameObject exitCell;
    [SerializeField] private GameObject door; 

    protected Dictionary<CellOrientation, List<GameObject>> cellsByOrientation;

    private void OnEnable() {
        cellsByOrientation = new Dictionary<CellOrientation, List<GameObject>>();
        cellsByOrientation.Add(CellOrientation.DeadEnd, deadEndCells);
        cellsByOrientation.Add(CellOrientation.Bend, bendCells);
        cellsByOrientation.Add(CellOrientation.Corridor, corridorCells);
        cellsByOrientation.Add(CellOrientation.T_Intersection, tIntersectionCells);
        cellsByOrientation.Add(CellOrientation.Intersection, intersectionCells);
    }

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellsByOrientation[orientation];
    }

    public GameObject StartingCell => startingCell;

    public GameObject ExitCell => exitCell;

    public GameObject Door => door;
}
