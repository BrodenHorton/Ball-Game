using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float baseHealth;
    public float baseArmor;
    public float baseSpeed;
    public float baseDamage;
    public string enemyName;
    public float baseAttackSpeed;
    public float baseAttackRange;
    public Projectile projectile;
}
