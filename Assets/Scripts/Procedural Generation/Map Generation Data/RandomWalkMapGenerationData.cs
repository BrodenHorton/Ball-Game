using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkMapGenerationData", menuName = "Scriptable Objects/Map Generation/Random Walk")]
public class RandomWalkMapGenerationData : ScriptableObject {
    [SerializeField] private bool hasBranchPaths;
    [SerializeField] private int randomWalkIterations;
    [SerializeField] private int randomWalkLength;
    [SerializeField] private int drunkWalkIterations;
    [SerializeField] private int drunkWalkLength;

    public bool HasBranchPaths => hasBranchPaths;

    public int RandomWalkIterations => randomWalkIterations;

    public int MaxRandomWalkLength => randomWalkLength;

    public int DrunkWalkIterations => drunkWalkIterations;

    public int MaxDrunkWalkLength => drunkWalkLength;
}
