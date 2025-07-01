using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static Action<GameObject> EnemyDeath;
    public static Action<GameObject> EnemyHit;
}
