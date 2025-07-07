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
}
