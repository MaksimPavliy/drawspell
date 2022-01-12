using System.Collections.Generic;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    public class SpellShapes : MonoBehaviour
    {
        private List<Shape> _shapes = new List<Shape>();
        public List<Shape> Shapes => _shapes;
        
        [SerializeField] private Transform m_shapesParent;

        private float _shapeDistance = 1.5f;

        public Shape FirstShape => Shapes[0];
        public void ClearShapes()
        {
            foreach (var shape in Shapes)
            {
                Destroy(shape.transform.gameObject);
            }
            Shapes.Clear();
        }
        public void CreateShape(ShapeType shapeType)
        {
            var shapeInfo = GameSettings.instance.GetShapeInfo(shapeType);
            var shape = Instantiate(shapeInfo.shape, m_shapesParent);
            shape.transform.position = transform.position;
            _shapes.Add(shape);
            UpdateShapesView();
        }

        public bool DisposeShape(ShapeType shapeType)
        {
            var shape = Shapes.Find(x => x.Type == shapeType);
            if (shape == null)
            {
                return false;
            }

            Shapes.Remove(shape);
            Destroy(shape.gameObject);
            UpdateShapesView();
            return true;
        }
        public void UpdateShapesView()
        {
            for (int i = 0; i < Shapes.Count; i++)
            {
                bool active = i == 0;
                if (active) Shapes[i].Animate();
            }
           
        }
    }
}