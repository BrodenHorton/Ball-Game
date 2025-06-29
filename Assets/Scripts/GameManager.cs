using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject playerInstance;
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
