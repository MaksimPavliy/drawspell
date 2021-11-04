using UnityEngine;

public interface IDamageable
{
    public delegate void OnDamage(IDamageable enemy);
    event OnDamage OnPlayerAttacked;

    GameObject gameObject { get; }

    void TakeDamage(Shape shape);
}
