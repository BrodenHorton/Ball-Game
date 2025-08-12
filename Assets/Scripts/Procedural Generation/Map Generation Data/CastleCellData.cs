using UnityEngine;

[CreateAssetMenu(fileName = "CastleCellData", menuName = "Scriptable Objects/Map Cell Data/Castle")]
public class CastleCellData : MapCellData {
    private static readonly string CASTLE_CELL_PATH = "Prefabs/Map/Map Cells/Castle/";

    private void OnEnable() {
        cellsByOrientation[CellOrientation.DeadEnd].Add(Resources.Load<GameObject>(CASTLE_CELL_PATH + "Dead Ends/Castle Dead End Cell 1"));
        cellsByOrientation[CellOrientation.Corridor].Add(Resources.Load<GameObject>(CASTLE_CELL_PATH + "Corridors/Castle Corridor Cell 1"));
        cellsByOrientation[CellOrientation.Bend].Add(Resources.Load<GameObject>(CASTLE_CELL_PATH + "Bends/Castle Bend Cell 1"));
        cellsByOrientation[CellOrientation.T_Intersection].Add(Resources.Load<GameObject>(CASTLE_CELL_PATH + "T-Intersections/Castle T-Intersection Cell 1"));
        cellsByOrientation[CellOrientation.Intersection].Add(Resources.Load<GameObject>(CASTLE_CELL_PATH + "Intersections/Castle Intersection Cell 1"));
    }
}