using UnityEngine;

[CreateAssetMenu(fileName = "RoomFirstMapGenerationData", menuName = "Scriptable Objects/Map Generation/Room First")]
public class RoomFirstMapGenerationData : MapGenerationData {
    [SerializeField] private int minRoomSize;
    [SerializeField] private int offset;

    public int MinRoomSize => minRoomSize;

    public int Offset => offset;
}