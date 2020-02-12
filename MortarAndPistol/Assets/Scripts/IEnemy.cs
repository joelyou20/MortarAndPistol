using UnityEngine;

public interface IEnemy
{
    //float __speed__ { get; set; }
    //int __health__ { get; set; }
    //int __baseDmg__ { get; set; }
    //float __attackRange__ = { get; set; }
    //float __attackSpeed__ = { get; set; }
    //List<Reagent> __drops__ = { get; set; }
    void Attack();
    void TakeDamage(int damage);
    void Die();
}
