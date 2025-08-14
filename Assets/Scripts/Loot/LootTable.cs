using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/Loot Table")]
public class LootTable : ScriptableObject {
    [SerializeField] private List<WeightedEntry<GameObject>> weightedLoot;

    public LootTable() {
        weightedLoot = new List<WeightedEntry<GameObject>>();
    }

    public List<WeightedEntry<GameObject>> WeightedLoot => weightedLoot;
}