using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    private static readonly System.Random RNG = new System.Random();

    [SerializeField] private Vector2Int gridDimensions;
    [SerializeField] private int gridCellSize;
    [SerializeField] private MapType mapType;

    private GridCell[,] gridCells;

    private void Start() {
        gridCells = new GridCell[gridDimensions.y, gridDimensions.x];

        GenerateGridCells();
        BuildMapCells();
        PrintGridCells();
    }

    private void GenerateGridCells() {
        Vector2Int startingCell = new Vector2Int(gridDimensions.x / 2, gridDimensions.y / 2);
        RandomWalkGeneration(startingCell);
        DrunkWalkBranchGeneration();
    }

    private void RandomWalkGeneration(Vector2Int startingCell) {
        gridCells[startingCell.y, startingCell.x] = new GridCell();

        Vector2Int currentCell;
        for (int i = 0; i < mapType.RandomWalkIterations; i++) {
            currentCell = startingCell;
            for (int j = 0; j < mapType.MaxRandomWalkLength; j++) {
                int direction = RNG.Next(0, 4);
                if (direction == 0 && currentCell.y - 1 >= 0) {
                    gridCells[currentCell.y, currentCell.x].walls[0] = false;
                    if (gridCells[currentCell.y - 1, currentCell.x] == null)
                        gridCells[currentCell.y - 1, currentCell.x] = new GridCell();
                    gridCells[currentCell.y - 1, currentCell.x].walls[2] = false;
                    currentCell.y--;
                }
                else if (direction == 1 && currentCell.x + 1 < gridCells.GetLength(1)) {
                    gridCells[currentCell.y, currentCell.x].walls[1] = false;
                    if (gridCells[currentCell.y, currentCell.x + 1] == null)
                        gridCells[currentCell.y, currentCell.x + 1] = new GridCell();
                    gridCells[currentCell.y, currentCell.x + 1].walls[3] = false;
                    currentCell.x++;
                }
                else if (direction == 2 && currentCell.y + 1 < gridCells.GetLength(0)) {
                    gridCells[currentCell.y, currentCell.x].walls[2] = false;
                    if (gridCells[currentCell.y + 1, currentCell.x] == null)
                        gridCells[currentCell.y + 1, currentCell.x] = new GridCell();
                    gridCells[currentCell.y + 1, currentCell.x].walls[0] = false;
                    currentCell.y++;
                }
                else if (direction == 3 && currentCell.x - 1 >= 0) {
                    gridCells[currentCell.y, currentCell.x].walls[3] = false;
                    if (gridCells[currentCell.y, currentCell.x - 1] == null)
                        gridCells[currentCell.y, currentCell.x - 1] = new GridCell();
                    gridCells[currentCell.y, currentCell.x - 1].walls[1] = false;
                    currentCell.x--;
                }
            }
        }
    }

    private void DrunkWalkBranchGeneration() {

    }

    private void BuildMapCells() {
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for(int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] == null)
                    continue;

                CellOrientation orientation = gridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(gridCells[i, j]);
                GameObject cell = Instantiate(mapType.GetCellsByOrientation(orientation)[0]);
                cell.transform.position = new Vector3(j * gridCellSize, 0f, i * -gridCellSize);
                cell.transform.Rotate(cell.transform.up, rotation);
            }
        }

        Debug.Log("Map construction finsihed");
    }

    private float getGridCellRotation(GridCell gridCell) {
        System.Random rand = new System.Random();
        CellOrientation orientation = gridCell.GetOrientation();
        float rotation = 0;
        if (orientation == CellOrientation.DeadEnd) {
            for (int i = 0; i < gridCell.walls.Length; i++) {
                if (!gridCell.walls[i]) {
                    rotation = i * 90;
                    break;
                }
            }
        }
        else if (orientation == CellOrientation.Corridor) {
            rotation = gridCell.walls[0] ? 90 : 0;
            rotation = rand.Next(0, 2) == 1 ? rotation + 180 : rotation;
        }
        else if (orientation == CellOrientation.Bend) {
            if(gridCell.walls[1] && gridCell.walls[2])
                rotation = 90;
            else if (gridCell.walls[2] && gridCell.walls[3])
                rotation = 180;
            else if (gridCell.walls[3] && gridCell.walls[0])
                rotation = 270;
        }
        else if (orientation == CellOrientation.Intersection) {
            rotation = rand.Next(0, 4) * 90;
        }
        else if (orientation == CellOrientation.T_Intersection) {
            for (int i = 0; i < gridCell.walls.Length; i++) {
                if (gridCell.walls[i]) {
                    rotation = i * 90;
                    break;
                }
            }
        }

        return rotation;
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

}
