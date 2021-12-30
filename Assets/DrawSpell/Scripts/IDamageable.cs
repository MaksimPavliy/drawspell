using System;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    public delegate void OnDamage(IDamageable damageable);
    public delegate void OnDied(IDamageable damageable);
    public interface IDamageable
    {
        public event OnDied Died;
        Transform Transform { get; }
        public int HP { get; }
        void TakeDamage(int damage);
    }

    public interface ISpellTarget
    {
        event OnDamage SpellCasted;
        public SpellShapes Shapes { get; }
        void TakeDamage(ShapeType shapeType);
    }
}