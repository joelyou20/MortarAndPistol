using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    /*
    [SerializeField] public int __health__ { get; set; }
    [SerializeField] public float __energy__ { get; set; }
    [SerializeField] public float __speed__ { get; set; }
    [SerializeField] public int __baseDmg__ { get; set; }
    [SerializeField] public float __jumpHeight__ { get; set; }
    [SerializeField] public int __armour__ { get; set; }
    */

    public int health;
    public float energy;
    public float speed;
    public int baseDmg;
    public float jumpHeight;
    public int armour;

    public Transform bodySprite;
    public Transform headSprite;

    private SpriteRenderer _headSpriteRenderer;
    private SpriteRenderer _bodySpriteRenderer;

    void Start()
    {
        /*
        __health__ = health;
        __energy__ = energy;
        __speed__ = speed;
        __baseDmg__ = baseDmg;
        __jumpHeight__ = jumpHeight;
        __armour__ = armour;
        */

        _headSpriteRenderer = bodySprite.GetComponent<SpriteRenderer>();
        _bodySpriteRenderer = headSprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckHp();
    }

    private void CheckHp()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        CheckHp(); 
        StartCoroutine("Flasher");
    }

    IEnumerator Flasher()
    {
        var bodyNormalColor = _bodySpriteRenderer.color;
        var headNormalColor = _headSpriteRenderer.color;
        _headSpriteRenderer.material.color = Color.red;
        _bodySpriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        _headSpriteRenderer.material.color = headNormalColor;
        _bodySpriteRenderer.material.color = bodyNormalColor;
        yield return new WaitForSeconds(.1f);
    }

    public void Die()
    {
        // Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
