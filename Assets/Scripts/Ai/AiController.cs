using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AIMovement))]
public class AiController : MonoBehaviour, IDamageable
{
    public bool IsDead => currentHealth <= 0;
    float currentHealth;
    float maxHealth;
    [SerializeField] EnemyData data;

    NavMeshAgent agent;
    AIMovement movement;
    Timer attackTimer;
    GameObject target;
    bool isWithinAttackRange => target != null && Vector3.Distance(movement.getTargetLocation(), transform.position) <= data.baseAttackRange;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = data.baseAttackRange;
        agent.speed = data.baseSpeed * 1; //depth
        movement = GetComponent<AIMovement>();
        attackTimer = new Timer(data.baseAttackSpeed);
        maxHealth = GetCalculatedHealth();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            agent.SetDestination(movement.getTargetLocation());
        if (isWithinAttackRange)
        {
            Debug.Log("Attacking");
            Attack();
        }
        else
        {
            attackTimer.Reset();
        }
    }
    public void Attack()
    {
        attackTimer.Update();
        if (attackTimer.IsFinished())
        {
            attackTimer.Reset();
            if(target.transform.GetParentOrSelf().TryGetComponent(out IDamageable damageable))
            {
                float depthPercentage = 1;
                damageable.TakeDamage(data.baseDamage * (1 + depthPercentage));
                Debug.Log("Attacking " + damageable);
            }
            else
            {
                Debug.LogWarning("Couldnt find damageable on " + target);
            }
        }
    }
    public void Heal(float amt)
    {
        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
    }

    public void TakeDamage(float amt, EffectType effectType = EffectType.NORMAL)
    {
        currentHealth -= amt;
        Debug.Log("Taking Dmg: " + amt);
        if (IsDead)
        {
            EventBus.EnemyDeath.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
    public float GetCalculatedHealth()
    {
        float depthPercentage = 1;
        return data.baseHealth * (1 + depthPercentage);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Trigger: " + other);
        target = other.gameObject;
        movement.SetTarget(target);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited Trigger: " + other);
        if (other.gameObject == target)
            target = null;
        movement.SetTarget(target);
    }
}
