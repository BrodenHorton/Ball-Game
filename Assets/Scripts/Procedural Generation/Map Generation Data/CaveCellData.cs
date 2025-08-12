using UnityEngine;

[CreateAssetMenu(fileName = "CaveCellData", menuName = "Scriptable Objects/Map Cell Data/Cave")]
public class CaveCellData : MapCellData {
    private static readonly string CAVE_CELL_PATH = "Prefabs/Map/Map Cells/Cave/";

    private void OnEnable() {
        cellsByOrientation[CellOrientation.DeadEnd].Add(Resources.Load<GameObject>(CAVE_CELL_PATH + "Dead Ends/Cave Dead End Cell 1"));
        cellsByOrientation[CellOrientation.Corridor].Add(Resources.Load<GameObject>(CAVE_CELL_PATH + "Corridors/Cave Corridor Cell 1"));
        cellsByOrientation[CellOrientation.Bend].Add(Resources.Load<GameObject>(CAVE_CELL_PATH + "Bends/Cave Bend Cell 1"));
        cellsByOrientation[CellOrientation.T_Intersection].Add(Resources.Load<GameObject>(CAVE_CELL_PATH + "T-Intersections/Cave T-Intersection Cell 1"));
        cellsByOrientation[CellOrientation.Intersection].Add(Resources.Load<GameObject>(CAVE_CELL_PATH + "Intersections/Cave Intersection Cell 1"));
    }
}
