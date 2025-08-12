using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class MapCellData : ScriptableObject {
    protected Dictionary<CellOrientation, List<GameObject>> cellsByOrientation;

    public MapCellData() {
        cellsByOrientation = new Dictionary<CellOrientation, List<GameObject>>();
        foreach (CellOrientation cellOrientation in Enum.GetValues(typeof(CellOrientation)))
            cellsByOrientation[cellOrientation] = new List<GameObject>();
    }

    public List<GameObject> GetCellsByOrientation(CellOrientation orientation) {
        return cellsByOrientation[orientation];
    }
}
