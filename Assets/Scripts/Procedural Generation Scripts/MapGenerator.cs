using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int gridDimensions;
    [SerializeField] private int gridCellSize;
    [SerializeField] private MapGenerationType generationType;

    private GridCell[,] gridCells;

    private void Start() {
        gridCells = new GridCell[gridDimensions.y, gridDimensions.x];

        GenerateGridCells();
        BuildMapCells();
    }

    private void GenerateGridCells() {
        Vector2Int startingCell = new Vector2Int(gridDimensions.y / 2, gridDimensions.x / 2);
        gridCells[startingCell.y, startingCell.x] = new GridCell();

        System.Random rng = new System.Random();
        Vector2Int currentCell;
        for(int i = 0; i < generationType.RandomWalkIterations; i++) {
            currentCell = startingCell;
            for (int j = 0; j < generationType.MaxRandomWalkLength; j++) {
                int direction = rng.Next(0, 4);
                if (direction == 0 && currentCell.y - 1 >= 0) {
                    gridCells[currentCell.y, currentCell.x].walls[0] = false;
                    if (gridCells[currentCell.y - 1, currentCell.x] == null)
                        gridCells[currentCell.y - 1, currentCell.x] = new GridCell();
                    gridCells[currentCell.y - 1, currentCell.x].walls[2] = false;
                    currentCell.y--;
                }
                else if(direction == 1 && currentCell.x + 1 < gridCells.GetLength(1)) {
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

    private void BuildMapCells() {
        string row = "";
        for (int i = 0; i < gridCells.GetLength(0); i++) {
            for(int j = 0; j < gridCells.GetLength(1); j++) {
                if (gridCells[i, j] == null) {
                    row = row + "x";
                    continue;
                }

                CellOrientation orientation = gridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(gridCells[i, j]);
                GameObject cell = Instantiate(MapCellRegistry.Instance.GetCellsBy(generationType.MapType, orientation)[0]);
                cell.transform.position = new Vector3(j * gridCellSize, 0f, i * -gridCellSize);
                cell.transform.Rotate(cell.transform.up, rotation);
                if(orientation == CellOrientation.DeadEnd) {
                    row += "D";
                }
                else if (orientation == CellOrientation.Corridor) {
                    row += "C";
                }
                else if (orientation == CellOrientation.Bend) {
                    row += "B";
                }
                else if (orientation == CellOrientation.T_Intersection) {
                    row += "T";
                }
                else if (orientation == CellOrientation.Intersection) {
                    row += "I";
                }
            }
            row += "\n";
        }

        Debug.Log(row);
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

}
