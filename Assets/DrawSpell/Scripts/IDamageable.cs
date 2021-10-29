using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public delegate void OnDamage(IDamageable enemy);
    event OnDamage OnDamageTaken;

    GameObject gameObject { get; }

    void TakeDamage(Spell spell);
}
