using System;

public class GridCell {
    public bool[] walls;

    public GridCell() {
        walls = new bool[4];
        for(int i = 0; i < walls.Length; i++)
            walls[i] = true;
    }

    public int GetNumberOfStandingWalls() {
        int count = 0;
        for (int i = 0; i < walls.Length; i++) {
            if (walls[i])
                count++;
        }

        return count;
    }

    public CellOrientation GetOrientation() {
        CellOrientation orientation = CellOrientation.Intersection;
        int standingWallCount = GetNumberOfStandingWalls();
        if (standingWallCount >= 3)
            orientation = CellOrientation.DeadEnd;
        else if (standingWallCount == 2) {
            orientation = walls[0] && walls[2] || walls[1] && walls[3] ? CellOrientation.Corridor : CellOrientation.Bend;
        }
        else if (standingWallCount == 1)
            orientation = CellOrientation.T_Intersection;

        return orientation;
    }
}
