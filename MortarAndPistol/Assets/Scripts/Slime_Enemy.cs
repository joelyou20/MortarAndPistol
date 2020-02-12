using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Enemy: Enemy
{
    public GameObject attackRangeCol;

    // Start is called before the first frame update
    void Start()
    {
        attackRangeCol.GetComponent<CircleCollider2D>().radius = attackRange;
        //attackRangeCol.layer = LayerMask.NameToLayer("Projectile");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void FixedUpdate()
    {
        Debug.Log(Mathf.RoundToInt(Time.time * 100) % (100 - attackSpeed));
        if (Mathf.RoundToInt(Time.time * 100) % (100 - attackSpeed) == 0)
        {
            if (IsPlayerWithinRange())
            {
                // Play attack animation
                Attack();
            }
        }
    }
}
