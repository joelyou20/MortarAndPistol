using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{

    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ //
    // ------------- ENEMY STATS ------------ //
    // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv //

    [SerializeField] public float speed = 5f;
    [SerializeField] public int health = 10;
    [SerializeField] public int baseDmg = 1;
    [SerializeField] public float attackRange = 1;
    [Range(0, 100)] [SerializeField] public int attackSpeed = 2;
    [SerializeField] public List<GameObject> drops;

    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ //
    // -------------------------------------- //
    // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv //

    private List<Collider2D> _colList;

    public bool IsPlayerWithinRange()
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.NameToLayer("Player"));
        _colList = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player")).ToList();

        return _colList.Count > 0;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {
        foreach (var col in _colList)
        {
            var player = col.gameObject.GetComponent<Player>();
            var player_rb = player.gameObject.GetComponent<Rigidbody2D>();
            player.TakeDamage(baseDmg);
            KnockBack(player_rb);
        }
    }

    private void KnockBack(Rigidbody2D prb)
    {
        var opposite = -prb.velocity;
        prb.AddForce(opposite * Time.deltaTime);
    }

    public void Die()
    {
        Instantiate(drops[0]);
        // Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
