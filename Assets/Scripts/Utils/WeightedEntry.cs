using System;
using UnityEngine;

[Serializable]
public class WeightedEntry<T> {
    [SerializeField] private T value;
    [SerializeField] private float weight;

    public WeightedEntry(T value, float weight) {
        this.value = value;
        this.weight = weight;
    }

    public T Value => value;

    public float Weight => weight;
}
