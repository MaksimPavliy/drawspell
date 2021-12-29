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
                transform.position = new Vector3(Target.gameObject.transform.position.x, Target.gameObject.transform.position.y - offset, Target.gameObject.transform.position.z);

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