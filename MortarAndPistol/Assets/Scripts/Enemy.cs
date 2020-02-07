using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{

    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ //
    // ------------- ENEMY STATS ------------ //
    // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv //

    [SerializeField] public float speed = 5f;
    [SerializeField] public int health = 10;
    [SerializeField] public int baseDmg = 1;

    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ //
    // -------------------------------------- //
    // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv //

    void Start()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Attack(col);
    }

    public void Attack(Collision2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        Debug.Log(col.gameObject.name);
        if (player)
        {
            Debug.Log(col.gameObject.name);
            var player_rb = col.gameObject.GetComponent<Rigidbody2D>();
            player.TakeDamage(baseDmg);
            KnockBack(player_rb);
        }

        // Hits something that is not a player
        // TODO...
    }

    private void KnockBack(Rigidbody2D prb)
    {
        var opposite = -prb.velocity;
        prb.AddForce(opposite * Time.deltaTime);
    }

    public void Die()
    {
        // Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
