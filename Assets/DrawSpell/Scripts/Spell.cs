using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    public abstract class Spell : MonoBehaviour
    {
        //    [SerializeField] private Shape shape;

        private bool dealedDamage = false;
        private ParticleSystem particles;
        private IDamageable target;
        private bool isCasted;
        public ShapeType ShapeType { set; get; }
        //  public Shape Shape => shape;
        public ParticleSystem Particles => particles;

        public bool DealedDamage { get => dealedDamage; set => dealedDamage = value; }
        public IDamageable Target { get => target; set => target = value; }
        public bool IsCasted { get => isCasted; set => isCasted = value; }
        public event Action<Spell> Disposed;

        void Start()
        {
            if (GetComponent<ParticleSystem>() != null)
            {
                particles = GetComponent<ParticleSystem>();
            }

            target.OnPlayerAttacked += SetSpellState;
        }

        private void OnDisable()
        {
            Disposed?.Invoke(this);
        }
        private void SetSpellState(IDamageable enemy)
        {
            if (Target == enemy)
            {
                dealedDamage = true;
            }
        }
    }
}