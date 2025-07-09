using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map {
    private GridCell[,] gridCells;
    private Vector2Int startingCell;
    private Vector2Int exitCell;
    private Dictionary<Vector2Int, int> depthByCell;

    public Map(Vector2Int gridDimensions) {
        gridCells = new GridCell[gridDimensions.y, gridDimensions.x];
        depthByCell = new Dictionary<Vector2Int, int>();
    }

    private void PrintGridCells() {
        string mapStr = "";
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for (int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] == null) {
                    mapStr = mapStr + "-";
                    continue;
                }

                CellOrientation orientation = gridCells[i, j].GetOrientation();
                if (orientation == CellOrientation.DeadEnd)
                    mapStr += "D";
                else if (orientation == CellOrientation.Corridor)
                    mapStr += "C";
                else if (orientation == CellOrientation.Bend)
                    mapStr += "B";
                else if (orientation == CellOrientation.T_Intersection)
                    mapStr += "T";
                else if (orientation == CellOrientation.Intersection)
                    mapStr += "I";
            }
            mapStr += "\n";
        }

        Debug.Log(mapStr);
    }

    public GridCell[,] GridCells { get { return gridCells; } set { gridCells = value; } }

    public Vector2Int StartingCell { get { return startingCell; } set { startingCell = value; } }

    public Vector2Int ExitCell { get { return exitCell; } set { exitCell = value; } }

    public Dictionary<Vector2Int, int> DepthByCell { get { return depthByCell; } set { depthByCell = value; } }
}
