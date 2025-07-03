using System;
using System.Collections.Generic;
using UnityEngine;

public class MapCellRegistry : MonoBehaviour {
    private static MapCellRegistry instance;

    private Dictionary<MapType, Dictionary<CellOrientation, List<GameObject>>> cellByMapTypeAndOrientation;

    private void Awake() {
        if(instance != null) {
            Debug.Log("Instance of MapCellRegistry already exists in scene.");
            Destroy(gameObject);
        }

        instance = this;

        cellByMapTypeAndOrientation = new Dictionary<MapType, Dictionary<CellOrientation, List<GameObject>>>();
        foreach(MapType mapType in Enum.GetValues(typeof(MapType))) {
            cellByMapTypeAndOrientation[mapType] = new Dictionary<CellOrientation, List<GameObject>>();
            foreach(CellOrientation cellOrientation in Enum.GetValues(typeof(CellOrientation))) {
                cellByMapTypeAndOrientation[mapType][cellOrientation] = new List<GameObject>();
            }
        }

        cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.DeadEnd].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Dead Ends/Cave Dead End Cell 1"));
        cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Corridor].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Corridors/Cave Corridor Cell 1"));
        cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Bend].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Bends/Cave Bend Cell 1"));
        cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.T_Intersection].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/T Intersections/Cave T-Intersection Cell 1"));
        cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Intersection].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Intersections/Cave Intersection Cell 1"));

        Debug.Log("Number of cells in Dead End: " + cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.DeadEnd].Count);
        Debug.Log("Number of cells in Corridor: " + cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Corridor].Count);
        Debug.Log("Number of cells in Bend: " + cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Bend].Count);
        Debug.Log("Number of cells in T Intersection: " + cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.T_Intersection].Count);
        Debug.Log("Number of cells in Intersection: " + cellByMapTypeAndOrientation[MapType.Cave][CellOrientation.Intersection].Count);
    }

    public static MapCellRegistry Instance => instance;

    public List<GameObject> GetCellsBy(MapType mapType, CellOrientation cellOrientation) {
        return cellByMapTypeAndOrientation[mapType][cellOrientation];
    }
}
