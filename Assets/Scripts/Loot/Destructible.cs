using UnityEngine;

public interface Destructible {
    void Break();

    bool ShouldBreak(Collider collider);
}
