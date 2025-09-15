using System;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

[Serializable]
public class WeightedList<T> {
    // Temp until world seed is set somewhere
    private static Random rng = new Random(Guid.NewGuid().GetHashCode());

    [SerializeField] private List<WeightedEntry<T>> weightedEntries;

    public WeightedList() {
        weightedEntries = new List<WeightedEntry<T>>();
    }

    public WeightedList(List<WeightedEntry<T>> weightedEntries) {
        this.weightedEntries = new List<WeightedEntry<T>> (weightedEntries);
    }

    public T GetWeightedValue() {
        int totalWeight = 0;
        for (int i = 0; i < weightedEntries.Count; i++)
            totalWeight += weightedEntries[i].Weight;

        T value = default;
        int weightIndex = rng.Next(1, totalWeight + 1);
        for (int i = 0; i < weightedEntries.Count; i++) {
            weightIndex -= weightedEntries[i].Weight;
            if (weightIndex <= 0) {
                value = weightedEntries[i].Value;
                break;
            }
        }

        return value;
    }

    private T GetWeightedValue(List<WeightedEntry<T>> weightedValues, out int index) {
        index = -1;
        int totalWeight = 0;
        for (int i = 0; i < weightedValues.Count; i++)
            totalWeight += weightedValues[i].Weight;

        T value = default;
        int weightIndex = rng.Next(1, totalWeight + 1);
        for (int i = 0; i < weightedValues.Count; i++) {
            weightIndex -= weightedValues[i].Weight;
            if (weightIndex <= 0) {
                value = weightedValues[i].Value;
                index = i;
                break;
            }
        }

        return value;
    }

    public List<T> GetWeightedValues(int min, int max) {
        return GetWeightedValues(rng.Next(min, max + 1));
    }

    public List<T> GetWeightedValues(int amt) {
        List<T> values = new List<T>();
        for (int i = 0; i < amt; i++) {
            T value = GetWeightedValue();
            if (value != null)
                values.Add(value);
        }

        return values;
    }

    public List<T> GetWeightedValuesNoRepeat(int min, int max) {
        return GetWeightedValuesNoRepeat(rng.Next(min, max + 1));
    }

    public List<T> GetWeightedValuesNoRepeat(int amt) {
        List<WeightedEntry<T>> weightedListCopy = new List<WeightedEntry<T>>(weightedEntries);
        List<T> results = new List<T>();
        for (int i = 0; i < amt && weightedListCopy.Count > 0; i++) {
            int index;
            T value = GetWeightedValue(weightedListCopy, out index);
            results.Add(value);
            weightedListCopy.RemoveAt(index);
        }

        return results;
    }

    public List<WeightedEntry<T>> WeightedEntries => weightedEntries;
}