using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpell : Spell
{
    [SerializeField] private float EmitionTimeBeforeDamage;
    [SerializeField] private float offset;

    void Update()
    {
        if (DealedDamage && !Particles.IsAlive())
        { 
            Destroy(gameObject);
        }

        if (!DealedDamage)
        {
            transform.position = new Vector3(Target.gameObject.transform.position.x, Target.gameObject.transform.position.y - offset, Target.gameObject.transform.position.z);

            if (Particles.time >= EmitionTimeBeforeDamage)
            {
                Target.TakeDamage(this);
            }
        }
    }
}
