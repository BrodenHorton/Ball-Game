using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIMovement))]
public class AiController : MonoBehaviour, IDamageable
{
    public bool IsDead => currentHealth <= 0;
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    NavMeshAgent agent;
    AIMovement movement;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<AIMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(movement.getTargetLocation());
    }
    public void Heal(float amt)
    {
        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
    }

    public void TakeDamage(float amt)
    {
        currentHealth -= amt;
        if (IsDead)
        {
            Destroy(gameObject);
        }
    }
}
