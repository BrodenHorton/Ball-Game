using UnityEngine;

public class RoomFirstGenerator : MapGenerator {
    [SerializeField] private MapGenerationData generationData;

    private System.Random rng;

    public override Map GenerateMap(int seed) {
        throw new System.NotImplementedException();
    }

    public override void BuildMapCells(Map map, Transform parent) {
        throw new System.NotImplementedException();
    }
}
