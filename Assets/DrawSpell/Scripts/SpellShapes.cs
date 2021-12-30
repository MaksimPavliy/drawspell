using System.Collections.Generic;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    public class SpellShapes : MonoBehaviour
    {
        private List<Shape> _shapes = new List<Shape>();
        public List<Shape> Shapes => _shapes;

        private float _shapeDistance = 1.5f;

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
            var shape = Instantiate(shapeInfo.shape, transform);
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
            Vector3 offset =Vector3.left* (Shapes.Count / 2f*_shapeDistance-_shapeDistance/2f);
            for (int i = 0; i < Shapes.Count; i++)
            {
                Shapes[i].transform.localPosition = offset + Vector3.right * _shapeDistance*i;
            }
           
        }
    }
}