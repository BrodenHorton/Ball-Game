using UnityEngine;

[CreateAssetMenu(fileName = "MapGenerationType", menuName = "Scriptable Objects/MapGenerationType")]
public class MapGenerationType : ScriptableObject {
    [SerializeField] private int randomWalkIterations;
    [SerializeField] private int minRandomWalkLength;
    [SerializeField] private int maxRandomWalkLength;
    [SerializeField] private int drunkWalkIterations;
    [SerializeField] private int minDrunkWalkLength;
    [SerializeField] private int maxDrunkWalkLength;
    [SerializeField] private MapType mapType;

    public int RandomWalkIterations => randomWalkIterations;

    public int MinRandomWalkLength => minRandomWalkLength;

    public int MaxRandomWalkLength => maxRandomWalkLength;

    public int DrunkWalkIterations => drunkWalkIterations;

    public int MinDrunkWalkLength => minDrunkWalkLength;

    public int MaxDrunkWalkLength => maxDrunkWalkLength;

    public MapType MapType => mapType;
}
