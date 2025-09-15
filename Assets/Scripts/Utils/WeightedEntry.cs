using System;
using UnityEngine;

[Serializable]
public class WeightedEntry<T> {
    [SerializeField] private T value;
    [SerializeField] private int weight;

    public WeightedEntry(T value, int weight) {
        this.value = value;
        this.weight = weight;
    }

    public T Value => value;

    public int Weight => weight;
}
