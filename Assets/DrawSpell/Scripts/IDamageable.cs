using UnityEngine;
using static Shape;

public interface IDamageable
{
    public delegate void OnDamage(IDamageable enemy);
    event OnDamage OnPlayerAttacked;

    GameObject gameObject { get; }

    void TakeDamage(ShapeType shapeType);
}
