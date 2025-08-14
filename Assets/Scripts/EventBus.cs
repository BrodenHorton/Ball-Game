using System;
using UnityEngine;

public static class EventBus
{
    public static Action<bool> Dashing;
    public static Action<GameObject> DashedInto;
    public static Action<AbilityData, int> AbilityAdded;
    public static Action<AbilityData, int> AbilityRemoved;
    public static Action<AbilityData, int> AbilityUpdated;
    public static Action<GameObject> EnemyDeath;
    public static Action<Destructible> OnDestructibleBreak;
}
