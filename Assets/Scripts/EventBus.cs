using System;
using UnityEditor.PackageManager;
using UnityEngine;

public static class EventBus
{
    public static Action<GameObject> EnemyDeath;
    public static Action<bool> Dashing;
    public static Action<GameObject> DashedInto;
    public static Action<Ability, int> AbilityAdded;
    public static Action<Ability, int> AbilityRemoved;
    public static Action<Ability, int> AbilityUpdated;
}
