using System.Collections.Generic;
using System;
using UnityEngine;
using Random = System.Random;

public class RoomFirstGenerator : MapGenerator {
    private static readonly int MAP_GRID_CELL_MARGIN = 1;

    [SerializeField] private RoomFirstMapGenerationData generationData;
    [SerializeField] private bool shouldGenerateRoomBoundingBoxes;
    [SerializeField] private bool shouldGenerateCorridors;

    private Random rng;

    public override Map GenerateMap(int seed) {
        rng = new Random(seed);
        Vector3 mapOrigin = new Vector3(-generationData.GridDimensions.x * generationData.GridCellSize / 2, 0f, generationData.GridDimensions.y * generationData.GridCellSize / 2);
        Map map = new Map(generationData.GridDimensions, mapOrigin, generationData.GridCellSize);
        map.GridCells = new GridCell[generationData.GridDimensions.y, generationData.GridDimensions.x];

        RoomFirstGeneration(map.GridCells, new BoundsInt(new Vector3Int(MAP_GRID_CELL_MARGIN, MAP_GRID_CELL_MARGIN), new Vector3Int(generationData.GridDimensions.x - MAP_GRID_CELL_MARGIN * 2, generationData.GridDimensions.y - MAP_GRID_CELL_MARGIN * 2)));
        PlaceStartingCell(map);
        CreateDepthMap(map);
        PlaceExitCell(map);

        return map;
    }

    private void RoomFirstGeneration(GridCell[,] gridCells, BoundsInt roomBounds) {
        List<BoundsInt> rooms = new List<BoundsInt>();
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();
        roomQueue.Enqueue(roomBounds);
        while(roomQueue.Count > 0) {
            BoundsInt room = roomQueue.Dequeue();
            if(room.size.x > generationData.MinRoomSize && room.size.y > generationData.MinRoomSize) {
                if(rng.NextDouble() >= 0.5) {
                    if(room.size.y >= generationData.MinRoomSize * 2) {
                        foreach (BoundsInt splitRoom in SplitHorizontally(room))
                            roomQueue.Enqueue(splitRoom);
                    }
                    else if(room.size.x >= generationData.MinRoomSize * 2) {
                        foreach (BoundsInt splitRoom in SplitVertically(room))
                            roomQueue.Enqueue(splitRoom);
                    }
                    else
                        rooms.Add(room);
                }
                else {
                    if (room.size.x >= generationData.MinRoomSize * 2) {
                        foreach (BoundsInt splitRoom in SplitVertically(room))
                            roomQueue.Enqueue(splitRoom);
                    }
                    else if (room.size.y >= generationData.MinRoomSize * 2) {
                        foreach (BoundsInt splitRoom in SplitHorizontally(room))
                            roomQueue.Enqueue(splitRoom);
                    }
                    else
                        rooms.Add(room);
                }
            }
            else
                rooms.Add(room);
        }

        for (int i = rooms.Count - 1; i >= 0; i--) {
            Vector2Int roomCenter = new Vector2Int(rooms[i].size.x - generationData.RoomOffset * 2, rooms[i].size.y - generationData.RoomOffset * 2);
            if (roomCenter.x <= 0 || roomCenter.y <= 0) {
                rooms.RemoveAt(i);
                continue;
            }
        }

        foreach (BoundsInt room in rooms) {
            if (shouldGenerateRoomBoundingBoxes) {
                for (int i = generationData.RoomOffset; i < room.size.y - generationData.RoomOffset; i++) {
                    for (int j = generationData.RoomOffset; j < room.size.x - generationData.RoomOffset; j++) {
                        InsertGridCell(gridCells, new Vector2Int(room.min.x + j, room.min.y + i));
                    }
                }
            }
            else {
                RandomWalkGeneration(gridCells, new BoundsInt(
                new Vector3Int(room.min.x + generationData.RoomOffset, room.min.y + generationData.RoomOffset),
                new Vector3Int(room.size.x - generationData.RoomOffset * 2, room.size.y - generationData.RoomOffset * 2)));
            }
        }

        if (shouldGenerateCorridors)
            CorridorGeneration(gridCells, rooms);
    }

    private List<BoundsInt> SplitHorizontally(BoundsInt room) {
        List<BoundsInt> rooms = new List<BoundsInt>();
        int ySplit = rng.Next(1, room.size.y);
        rooms.Add(new BoundsInt(new Vector3Int(room.min.x, room.min.y), new Vector3Int(room.size.x, ySplit)));
        rooms.Add(new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit), new Vector3Int(room.size.x, room.size.y - ySplit)));

        return rooms;
    }

    private List<BoundsInt> SplitVertically(BoundsInt room) {
        List<BoundsInt> rooms = new List<BoundsInt>();
        int xSplit = rng.Next(1, room.size.x);
        rooms.Add(new BoundsInt(new Vector3Int(room.min.x, room.min.y), new Vector3Int(xSplit, room.size.y)));
        rooms.Add(new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y), new Vector3Int(room.size.x - xSplit, room.size.y)));

        return rooms;
    }

    private void RandomWalkGeneration(GridCell[,] gridCells, BoundsInt room) {
        Vector2Int roomCenter = new Vector2Int(room.min.x + room.size.x / 2, room.min.y + room.size.y / 2);
        if ( gridCells[roomCenter.y, roomCenter.x] == null)
            InsertGridCell(gridCells, roomCenter);

        int randomWalkIterations = 5;
        int maxRandomWalkLength = 10;
        Vector2Int currentCell;
        for (int i = 0; i < randomWalkIterations; i++) {
            currentCell = roomCenter;
            for (int j = 0; j < maxRandomWalkLength; j++) {
                Array cardinalDirections = Enum.GetValues(typeof(Direction2D));
                Direction2D direction = (Direction2D)cardinalDirections.GetValue(rng.Next(cardinalDirections.Length));
                Vector2Int nextCell = new Vector2Int(currentCell.x + direction.Vector().x, currentCell.y + direction.Vector().y);
                if (nextCell.x < room.min.x || nextCell.x > room.max.x || nextCell.y < room.min.y || nextCell.y > room.max.y)
                    continue;

                InsertGridCell(gridCells, nextCell);
                currentCell = nextCell;
            }
        }
    }

    private void CorridorGeneration(GridCell[,] gridCells, List<BoundsInt> rooms) {
        List<BoundsInt> remainingRooms = new List<BoundsInt>(rooms);
        remainingRooms.Shuffle(rng);
        BoundsInt selectedRoom = remainingRooms[0];
        remainingRooms.Remove(selectedRoom);
        while(remainingRooms.Count > 0) {
            BoundsInt nextRoom = selectedRoom;
            int minDistance = int.MaxValue;
            foreach(BoundsInt room in remainingRooms) {
                int distance = Math.Abs((selectedRoom.min.x + selectedRoom.size.x / 2) - (room.min.x + selectedRoom.size.x / 2)) + Math.Abs((selectedRoom.min.y + selectedRoom.size.y / 2) - (room.min.y + selectedRoom.size.y / 2));
                if(distance < minDistance) {
                    nextRoom = room;
                    minDistance = distance;
                }
            }

            Vector2Int currentCell = new Vector2Int(selectedRoom.min.x + selectedRoom.size.x / 2, selectedRoom.min.y + selectedRoom.size.y / 2);
            Vector2Int targetCell = new Vector2Int(nextRoom.min.x + nextRoom.size.x / 2, nextRoom.min.y + nextRoom.size.y / 2);
            if (rng.NextDouble() < 0.5) {
                while(currentCell.x != targetCell.x) {
                    if(currentCell.x < targetCell.x) {
                        currentCell.x++;
                        InsertGridCell(gridCells, currentCell);
                    }
                    else {
                        currentCell.x--;
                        InsertGridCell(gridCells, currentCell);
                    }
                }

                while (currentCell.y != targetCell.y) {
                    if (currentCell.y < targetCell.y) {
                        currentCell.y++;
                        InsertGridCell(gridCells, currentCell);
                    }
                    else {
                        currentCell.y--;
                        InsertGridCell(gridCells, currentCell);
                    }
                }
            }
            else {
                while (currentCell.y != targetCell.y) {
                    if (currentCell.y < targetCell.y) {
                        currentCell.y++;
                        InsertGridCell(gridCells, currentCell);
                    }
                    else {
                        currentCell.y--;
                        InsertGridCell(gridCells, currentCell);
                    }
                }

                while (currentCell.x != targetCell.x) {
                    if (currentCell.x < targetCell.x) {
                        currentCell.x++;
                        InsertGridCell(gridCells, currentCell);
                    }
                    else {
                        currentCell.x--;
                        InsertGridCell(gridCells, currentCell);
                    }
                }
            }

            remainingRooms.Remove(selectedRoom);
            selectedRoom = nextRoom;
        }
    }

    protected override void PlaceStartingCell(Map map) {
        for (int i = map.GridCells.GetLength(0) - 1; i >= 0; i--) {
            for (int j = 0; j < map.GridCells.GetLength(1); j++) {
                if (map.GridCells[i, j] != null && i + 1 < map.GridCells.GetLength(0)) {
                    Vector2Int currentCell = new Vector2Int(j, i + 1);
                    InsertGridCell(map.GridCells, currentCell);
                    map.StartingCell = currentCell;
                    return;
                }
            }
        }
    }

    protected override void PlaceExitCell(Map map) {
        int maxDepthValue = 0;
        foreach (KeyValuePair<Vector2Int, int> entry in map.DepthByCell) {
            if (entry.Value > maxDepthValue)
                maxDepthValue = entry.Value;
        }
        int exitDepthThreshold = maxDepthValue / 2;
        List<Vector2Int> exitRootCells = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, int> entry in map.DepthByCell) {
            if (entry.Value >= exitDepthThreshold)
                exitRootCells.Add(entry.Key);
        }

        exitRootCells.Shuffle(rng);

        bool isExitCellSet = false;
        foreach (Vector2Int exitRootCell in exitRootCells) {
            foreach (Direction2D direction in Enum.GetValues(typeof(Direction2D))) {
                if (!map.GridCells[exitRootCell.y, exitRootCell.x].walls[(int)direction])
                    continue;
                Vector2Int exitCell = new Vector2Int(exitRootCell.x + direction.Vector().x, exitRootCell.y + direction.Vector().y);
                if (!IsGridIndexInBounds(map.GridCells, exitCell))
                    continue;
                if (map.GridCells[exitCell.y, exitCell.x] != null)
                    continue;
                if (NumberOfAdjacentCells(map.GridCells, exitCell) != 1)
                    continue;

                map.GridCells[exitRootCell.y, exitRootCell.x].walls[(int)direction] = false;
                map.GridCells[exitCell.y, exitCell.x] = new GridCell();
                map.GridCells[exitCell.y, exitCell.x].walls[((int)direction + 2) % 4] = false;
                map.ExitCell = exitCell;
                map.DepthByCell.Add(exitCell, map.DepthByCell[exitRootCell] + 1);
                isExitCellSet = true;
                break;
            }

            if (isExitCellSet)
                break;
        }
    }

    private void InsertGridCell(GridCell[,] gridCells, Vector2Int cell) {
        if (!IsGridIndexInBounds(gridCells, cell))
            return;
        
        if (gridCells[cell.y, cell.x] == null)
            gridCells[cell.y, cell.x] = new GridCell();

        foreach(Direction2D direction in typeof(Direction2D).GetEnumValues()) {
            Vector2Int adjacentCell = new Vector2Int(cell.x + direction.Vector().x, cell.y + direction.Vector().y);
            if(!IsGridIndexInBounds(gridCells, adjacentCell) || gridCells[adjacentCell.y, adjacentCell.x] == null)
                continue;

            gridCells[cell.y, cell.x].walls[((int)direction)] = false;
            gridCells[adjacentCell.y, adjacentCell.x].walls[((int)direction + 2) % 4] = false;
        }
    }

    public override void BuildMapCells(Map map, Transform parent) {
        for (int i = 0; i < map.GridCells.GetLength(0); i++) {
            for (int j = 0; j < map.GridCells.GetLength(1); j++) {
                if (map.GridCells[i, j] == null)
                    continue;

                CellOrientation orientation = map.GridCells[i, j].GetOrientation();
                float rotation = getGridCellRotation(map.GridCells[i, j], rng);
                GameObject cell;
                if (i == map.StartingCell.y && j == map.StartingCell.x)
                    cell = Instantiate(generationData.GetStartingCell(), parent);
                else if (i == map.ExitCell.y && j == map.ExitCell.x)
                    cell = Instantiate(generationData.GetExitCell(), parent);
                else
                    cell = Instantiate(generationData.GetCellsByOrientation(orientation)[0], parent);
                Vector3 cellCenter = new Vector3(j * generationData.GridCellSize + map.MapOrigin.x, map.MapOrigin.y, i * -generationData.GridCellSize + map.MapOrigin.z);
                cell.transform.localPosition = cellCenter;
                cell.transform.Rotate(cell.transform.up, rotation);

                if (generationData.GetDoor() != null) {
                    if (!map.GridCells[i, j].walls[1]) {
                        GameObject cellDoor = Instantiate(generationData.GetDoor(), parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x + generationData.GridDimensions.x / 2, cellCenter.y, cellCenter.z);
                        cellDoor.transform.Rotate(0f, 90f, 0f);
                    }
                    if (!map.GridCells[i, j].walls[2]) {
                        GameObject cellDoor = Instantiate(generationData.GetDoor(), parent);
                        cellDoor.transform.localPosition = new Vector3(cellCenter.x, cellCenter.y, cellCenter.z - generationData.GridDimensions.y / 2);
                    }
                }

                if (hasDepthValueSprites) {
                    GameObject cellDepthValue = Instantiate(depthValueSprite, parent);
                    cellDepthValue.transform.localPosition = cellCenter;
                    TextMesh textMesh = cellDepthValue.GetComponent<TextMesh>();
                    if (textMesh != null)
                        textMesh.text = map.DepthByCell[new Vector2Int(j, i)].ToString();
                }
            }
        }
    }
}
