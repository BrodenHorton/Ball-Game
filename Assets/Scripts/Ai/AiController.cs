using UnityEngine;
using UnityEngine.AI;

public abstract class AIController : MonoBehaviour, IDamageable
{
    public bool IsDead => currentHealth <= 0;
    protected float currentHealth;
    protected float maxHealth;
    [SerializeField] protected EnemyData data;

    protected NavMeshAgent agent;
    protected AIMovement movement;
    protected Timer attackTimer;
    protected GameObject target;
    protected bool isAttacking;
    protected bool isWithinAttackRange => target != null && Vector3.Distance(movement.getTargetLocation(), transform.position) <= data.baseAttackRange;
    protected virtual void Awake()
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
    protected virtual void Update()
    {
        if (isWithinAttackRange || isAttacking)
        {
            //Debug.Log("Attacking");
            Attack();
        }
        else if (!isAttacking)
        {
            attackTimer.Reset();
            movement.StartMoving();
        }
    }
    protected abstract void Attack();
    public virtual void Heal(float amt)
    {
        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
    }

    public virtual void TakeDamage(float amt, EffectType effectType = EffectType.NORMAL)
    {
        currentHealth -= amt;
        Debug.Log("Taking Dmg: " + amt);
        if (IsDead)
        {
            EventBus.EnemyDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
    public virtual float GetCalculatedHealth()
    {
        float depthPercentage = 1;
        return data.baseHealth * (1 + depthPercentage);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Trigger: " + other);
        target = other.gameObject;
        movement.SetTarget(target);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited Trigger: " + other);
        if (other.gameObject == target)
            target = null;
        movement.SetTarget(target);
    }
}
