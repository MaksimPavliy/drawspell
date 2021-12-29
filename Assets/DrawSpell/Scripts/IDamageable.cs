using System;
using UnityEngine;
using static Shape;

namespace DrawSpell {
    public interface IDamageable
    {
        public delegate void OnDamage(IDamageable damageable);
        public delegate void OnDied(IDamageable damageable);
        event OnDamage OnPlayerAttacked;
        public event OnDied Died;
        GameObject gameObject { get; }
        public int HP { get; }
        public HPShapes Shapes { get; }
        void TakeDamage(ShapeType shapeType);
    }
}