using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private Shape shape;
    [SerializeField] private bool dealedDamage = false;

    private ParticleSystem particles;
    private IDamageable target;
    private bool isCasted;

    public Shape Shape => shape;
    public ParticleSystem Particles => particles;

    public bool DealedDamage { get => dealedDamage; set => dealedDamage = value; }
    public IDamageable Target { get => target; set => target = value; }
    public bool IsCasted { get => isCasted; set => isCasted = value; }

    void Start()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            particles = GetComponent<ParticleSystem>();
        }

        target.OnPlayerAttacked += SetSpellState;
    }

    private void SetSpellState(IDamageable enemy)
    {
        if (Target == enemy)
        {
            dealedDamage = true;
        }
    }
}
