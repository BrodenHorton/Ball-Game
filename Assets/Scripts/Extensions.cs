using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Transform GetParentOrSelf(this Transform self)
    {
        return self.parent ? self.parent : self;
    }
    public static void ApplyForceInRandomUpDirection(this Rigidbody rb, float force)
    {
        rb.AddForce(Random.insideUnitSphere * force + Vector3.up);
    }
    public static void ToggleActiveIfChildrenExist(this GameObject self)
    {
        self.SetActive(self.transform.childCount > 0);
    }

    // rng is a temporary parameter until a class is made to hold map seeds and random number generators
    public static void Shuffle<T>(this List<T> list, System.Random rng) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
