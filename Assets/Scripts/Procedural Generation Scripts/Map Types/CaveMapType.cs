using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CaveMapType", menuName = "Scriptable Objects/Map Types/Cave")]
public class CaveMapType : MapType {
    
    private void OnEnable() {
        cellsByOrientation[CellOrientation.DeadEnd].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Dead Ends/Cave Dead End Cell 1"));
        cellsByOrientation[CellOrientation.Corridor].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Corridors/Cave Corridor Cell 1"));
        cellsByOrientation[CellOrientation.Bend].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Bends/Cave Bend Cell 1"));
        cellsByOrientation[CellOrientation.T_Intersection].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/T Intersections/Cave T-Intersection Cell 1"));
        cellsByOrientation[CellOrientation.Intersection].Add(Resources.Load<GameObject>("Prefabs/Map Cells/Cave/Intersections/Cave Intersection Cell 1"));
    }

}
