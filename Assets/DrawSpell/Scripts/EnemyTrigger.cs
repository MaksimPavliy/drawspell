using System;
using UnityEngine;

namespace DrawSpell
{
    public class EnemyTrigger: MonoBehaviour
    {
        public event Action<EnemyTrigger> Triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Triggered?.Invoke(this);
            }   
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position,0.3f);
        }
    }
}