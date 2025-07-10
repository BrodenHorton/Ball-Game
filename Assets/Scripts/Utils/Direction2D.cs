using System.Collections.Generic;
using UnityEngine;

public enum Direction2D {
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public static class Direction2DExtensions {
    private static Dictionary<Direction2D, Vector2Int> vectorByDirection;

    static Direction2DExtensions() {
        vectorByDirection = new Dictionary<Direction2D, Vector2Int>();
        vectorByDirection[Direction2D.North] = new Vector2Int(0, -1);
        vectorByDirection[Direction2D.East] = new Vector2Int(1, 0);
        vectorByDirection[Direction2D.South] = new Vector2Int(0, 1);
        vectorByDirection[Direction2D.West] = new Vector2Int(-1, 0);
    }

    public static Vector2Int Vector(this Direction2D direction) {
        return vectorByDirection[direction];
    }
}
