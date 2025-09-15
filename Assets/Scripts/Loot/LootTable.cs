using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/Loot Table")]
public class LootTable : ScriptableObject {
    [SerializeField] private WeightedList<GameObject> weightedLoot;

    public LootTable() {
        weightedLoot = new WeightedList<GameObject>();
    }

    public WeightedList<GameObject> WeightedLoot => weightedLoot;
}