using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public static LootGenerator Instance;
    [SerializeField] int creatureWorth;
    [SerializeField] GameObject soul;
    [SerializeField] float lootForce;
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
    private void Start()
    {
        EventBus.EnemyDeath += GenerateLootForCreature;
    }

    public void GenerateLootForCreature(GameObject creature)
    {
        for(int i = 0; i < creatureWorth; i++)
        {
            var c = Instantiate(soul, creature.transform.position, Quaternion.identity);
            c.GetComponent<Rigidbody>().ApplyForceInRandomUpDirection(lootForce);
        }
    }
}
