using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class EnemyTrigger: MonoBehaviour
    {
        public event Action<EnemyTrigger> Triggered;
        [SerializeField] private EnemyType m_type;
        [SerializeField] private List<Shape.ShapeType> m_shapes;

        public List<Shape.ShapeType> Shapes => m_shapes;
        public void SetShapes(List<Shape.ShapeType> shapes)
        {
            m_shapes = shapes;
        }

        
        public EnemyType Type => m_type;
        public void SetType(EnemyType type) => m_type = type;

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