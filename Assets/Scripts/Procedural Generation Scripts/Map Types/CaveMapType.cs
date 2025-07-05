using UnityEngine;

[CreateAssetMenu(fileName = "CaveMapType", menuName = "Scriptable Objects/Map Types/Cave")]
public class CaveMapType : MapType {
    private static readonly string MAP_CELL_PATH = "Prefabs/Map/Map Cells/";

    private void OnEnable() {
        cellsByOrientation[CellOrientation.DeadEnd].Add(Resources.Load<GameObject>(MAP_CELL_PATH + "Cave/Dead Ends/Cave Dead End Cell 1"));
        cellsByOrientation[CellOrientation.Corridor].Add(Resources.Load<GameObject>(MAP_CELL_PATH + "Cave/Corridors/Cave Corridor Cell 1"));
        cellsByOrientation[CellOrientation.Bend].Add(Resources.Load<GameObject>(MAP_CELL_PATH + "Cave/Bends/Cave Bend Cell 1"));
        cellsByOrientation[CellOrientation.T_Intersection].Add(Resources.Load<GameObject>(MAP_CELL_PATH + "Cave/T Intersections/Cave T-Intersection Cell 1"));
        cellsByOrientation[CellOrientation.Intersection].Add(Resources.Load<GameObject>(MAP_CELL_PATH + "Cave/Intersections/Cave Intersection Cell 1"));
    }

}
