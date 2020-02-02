using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float travelDistance = 5f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, travelDistance);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        EnemyAI enemy = col.GetComponent<EnemyAI>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (col.tag != "Player")
        {
            // Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
