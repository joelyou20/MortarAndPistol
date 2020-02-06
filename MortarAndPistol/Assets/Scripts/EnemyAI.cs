using UnityEngine;

public class EnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] public float __speed__ { get; set; }
    [SerializeField] public int __health__ { get; set; }

    public float speed = 5f;
    public int health = 10;

    void Start()
    {
        __speed__ = speed;
        __health__ = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
