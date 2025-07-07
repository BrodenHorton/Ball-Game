using System;
using System.Collections.Generic;

public class WeightedValues {

    // Temp until world seed is set somewhere
    private static System.Random rng = new System.Random(Guid.NewGuid().GetHashCode());

    public static T GetWeightedValue<T>(List<WeightedEntry<T>> weightedList, System.Random rng = null) {
        rng ??= WeightedValues.rng;
        float totalWeight = 0f;
        for (int i = 0; i < weightedList.Count; i++)
            totalWeight += weightedList[i].Weight;

        T value = default;
        float weightIndex = (float)rng.NextDouble() * totalWeight;
        for (int i = 0; i < weightedList.Count; i++) {
            weightIndex -= weightedList[i].Weight;
            if (weightIndex <= 0) {
                value = weightedList[i].Value;
                break;
            }
        }

        return value;
    }

    public static List<T> GetWeightedVAlues<T>(List<WeightedEntry<T>> weightedList, int amt) {
        List<T> values = new List<T>();
        for (int i = 0; i < amt; i++) {
            T value = GetWeightedValue(weightedList);
            if (value != null)
                values.Add(value);
        }

        return values;
    }

    public static List<T> GetWeightedVAlues<T>(List<WeightedEntry<T>> weightedList, int min, int max, System.Random rng = null) {
        rng ??= WeightedValues.rng;
        return GetWeightedVAlues(weightedList, rng.Next(min, max + 1));
    }

}
