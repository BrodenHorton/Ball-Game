using System;
using UnityEngine;

public static class EventBus
{
    public static Action<GameObject> EnemyDeath;
    public static Action<GameObject> DashedInto;
}
