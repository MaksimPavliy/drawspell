using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private List<LeanShape> spellSymbols;

    private bool isCasted;
    private IDamageable target;
    private ParticleSystem particles;
    private bool dealedDamage;

    public List<LeanShape> SpellSymbols => spellSymbols;
    public ParticleSystem Particles => particles;
    public bool DealedDamage => dealedDamage;

    public IDamageable Target { get => target; set => target = value; }
    public bool IsCasted { get => isCasted; set => isCasted = value; }

    void Start()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            particles = GetComponent<ParticleSystem>();
        }

        target.OnDamageTaken += SetSpellState;
    }

    private void SetSpellState(IDamageable enemy)
    {
        if (Target == enemy)
        {
            dealedDamage = true;
        }
    }
}
