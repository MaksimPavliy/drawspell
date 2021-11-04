using UnityEngine;

public class TargetSpell : Spell
{
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float offsetByY = 1;

    private Vector3 targetPosition;

    void Update()
    {
        if (IsCasted && !DealedDamage)
        {
            if (Vector3.Distance(transform.position, Target.gameObject.transform.position) < 2f)
            {
                Target.TakeDamage(Shape);
                DealedDamage = true;
            }
            else
            {
                targetPosition = Target.gameObject.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y + offsetByY, targetPosition.z), flyingSpeed);
            }
        }
        else
            gameObject.SetActive(false);
    }
}

