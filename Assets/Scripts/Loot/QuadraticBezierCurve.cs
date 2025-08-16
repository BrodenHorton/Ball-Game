using UnityEngine;

public class QuadraticBezierCurve {
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;

    public QuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2) {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    public Vector3 GetPosition(float t) {
        Mathf.Clamp(t, 0, 1);
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }
}
