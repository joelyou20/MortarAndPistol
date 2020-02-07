using UnityEngine;

public interface IEnemy
{
    //float __speed__ { get; set; }
    //int __health__ { get; set; }
    //int __baseDmg__ { get; set; }
    void Attack(Collision2D col);
    void TakeDamage(int damage);
    void Die();
}
