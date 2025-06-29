using System;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action<GameObject> EnemyDeath;
    private GameObject playerInstance;
    private int money;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    public void AddCoin(int value)
    {
        money += value;
        Debug.Log("Collected " + value + " total money is: $" + money);
    }

    public GameObject getPlayer()
    {
        if (playerInstance == null)
        {
            Debug.Log("Finding player by tag..");
            playerInstance = GameObject.FindGameObjectWithTag("Player");
        }
        return playerInstance;
    }
}
