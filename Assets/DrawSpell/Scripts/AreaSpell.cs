using UnityEngine;

namespace DrawSpell
{
    public class AreaSpell : Spell
    {
        [SerializeField] private float EmitionTimeBeforeDamage;
        [SerializeField] private float offset;

        void Update()
        {
            if (!DealedDamage)
            {
                transform.position = new Vector3(Damageable.Transform.position.x, Damageable.Transform.position.y - offset, Damageable.Transform.position.z);

                if (Particles.time >= EmitionTimeBeforeDamage)
                {
                  Target.TakeDamage(ShapeType);
                   DealedDamage = true;
                }
            }
            else if (!Particles.IsAlive())
            {
                gameObject.SetActive(false);
            }
        }
    }
}