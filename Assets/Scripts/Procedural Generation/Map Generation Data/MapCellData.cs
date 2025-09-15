using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapCellData", menuName = "Scriptable Objects/Map Cell Data")]
public class MapCellData : ScriptableObject {
    [Header("Cells")]
    [SerializeField] private WeightedList<GameObject> deadEndCells;
    [SerializeField] private WeightedList<GameObject> bendCells;
    [SerializeField] private WeightedList<GameObject> corridorCells;
    [SerializeField] private WeightedList<GameObject> tIntersectionCells;
    [SerializeField] private WeightedList<GameObject> intersectionCells;
    [Header("POI Cells")]
    [SerializeField] private WeightedList<GameObject> poiDeadEndCells;
    [SerializeField] private WeightedList<GameObject> poiBendCells;
    [SerializeField] private WeightedList<GameObject> poiCorridorCells;
    [SerializeField] private WeightedList<GameObject> poiTIntersectionCells;
    [SerializeField] private WeightedList<GameObject> poiIntersectionCells;
    [Header("Unique POI Cells")]
    [SerializeField] private List<GameObject> uniquePoiDeadEndCells;
    [SerializeField] private List<GameObject> uniquePoiBendCells;
    [SerializeField] private List<GameObject> uniquePoiCorridorCells;
    [SerializeField] private List<GameObject> uniquePoiTIntersectionCells;
    [SerializeField] private List<GameObject> uniquePoiIntersectionCells;
    [Header("Other Cell Data")]
    [SerializeField] private GameObject startingCell;
    [SerializeField] private GameObject exitCell;
    [SerializeField] private GameObject door;

    protected Dictionary<CellOrientation, WeightedList<GameObject>> cellsByOrientation;
    protected Dictionary<CellOrientation, WeightedList<GameObject>> poiCellsByOrientation;
    protected Dictionary<CellOrientation, List<GameObject>> uniquePoiCellsByOrientation;

    private void OnEnable() {
        cellsByOrientation = new Dictionary<CellOrientation, WeightedList<GameObject>>();
        cellsByOrientation.Add(CellOrientation.DeadEnd, deadEndCells);
        cellsByOrientation.Add(CellOrientation.Bend, bendCells);
        cellsByOrientation.Add(CellOrientation.Corridor, corridorCells);
        cellsByOrientation.Add(CellOrientation.T_Intersection, tIntersectionCells);
        cellsByOrientation.Add(CellOrientation.Intersection, intersectionCells);

        poiCellsByOrientation = new Dictionary<CellOrientation, WeightedList<GameObject>>();
        poiCellsByOrientation.Add(CellOrientation.DeadEnd, poiDeadEndCells);
        poiCellsByOrientation.Add(CellOrientation.Bend, poiBendCells);
        poiCellsByOrientation.Add(CellOrientation.Corridor, poiCorridorCells);
        poiCellsByOrientation.Add(CellOrientation.T_Intersection, poiTIntersectionCells);
        poiCellsByOrientation.Add(CellOrientation.Intersection, poiIntersectionCells);

        uniquePoiCellsByOrientation = new Dictionary<CellOrientation, List<GameObject>>();
        uniquePoiCellsByOrientation.Add(CellOrientation.DeadEnd, uniquePoiDeadEndCells);
        uniquePoiCellsByOrientation.Add(CellOrientation.Bend, uniquePoiBendCells);
        uniquePoiCellsByOrientation.Add(CellOrientation.Corridor, uniquePoiCorridorCells);
        uniquePoiCellsByOrientation.Add(CellOrientation.T_Intersection, uniquePoiTIntersectionCells);
        uniquePoiCellsByOrientation.Add(CellOrientation.Intersection, uniquePoiIntersectionCells);
    }

    public WeightedList<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellsByOrientation[orientation];
    }

    public WeightedList<GameObject> GetPoiCellsByOrientation(CellOrientation orientation) {
        return poiCellsByOrientation[orientation];
    }

    public List<GameObject> GetUniquePoiCellsByOrientation(CellOrientation orientation) {
        return uniquePoiCellsByOrientation[orientation];
    }

    public GameObject StartingCell => startingCell;

    public GameObject ExitCell => exitCell;

    public GameObject Door => door;
}