using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject playerInstance;
    private int souls;
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
    public void GainSoul(int value)
    {
        souls += value;
        Debug.Log("Collected " + value + " total souls is: " + souls);
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
