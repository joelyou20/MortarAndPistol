public interface IEnemyAI
{
    float __speed__ { get; set; }
    int __health__ { get; set; }
    void TakeDamage(int damage);
    void Die();
}
