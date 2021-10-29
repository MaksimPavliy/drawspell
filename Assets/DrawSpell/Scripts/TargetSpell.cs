using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpell : Spell
{
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float offsetByY = 1;

    private Vector3 targetPosition;

    void Update()
    {
        if (DealedDamage)
        {
            Destroy(gameObject);
        }

        if (IsCasted && !DealedDamage)
        {
            if (Target != null && Vector3.Distance(transform.position, Target.gameObject.transform.position) < 2f)
            {
                Target.TakeDamage(this);
            }
            else
            {
                targetPosition = Target.gameObject.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y + offsetByY, targetPosition.z), flyingSpeed);
            }
        }
    }
}
