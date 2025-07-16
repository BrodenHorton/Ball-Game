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
    public static Vector3 GetRandomDirectionWithinCone(this Vector3 direction, float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float z = Mathf.Cos(angleRadians * Random.value);
        float sinT = Mathf.Sin(angleRadians * Random.value);
        float phi = Random.Range(0, Mathf.PI * 2);
        float x = sinT * Mathf.Cos(phi);
        float y = sinT * Mathf.Sin(phi);

        Vector3 localDirection = new Vector3(x, y, z); // z is "forward" in cone
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);
        return rotation * localDirection;
    }
}
