using UnityEngine;

[CreateAssetMenu(fileName = "RoomFirstMapGenerationData", menuName = "Scriptable Objects/Map Generation/Room First")]
public class RoomFirstMapGenerationData : ScriptableObject {
    [SerializeField] private int minRoomSize;
    [SerializeField] private int roomOffset;

    public int MinRoomSize => minRoomSize;

    public int RoomOffset => roomOffset;
}