using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private ShapeType type;

    private bool isEnabled = true;

    public bool IsEnabled => isEnabled;
    public ShapeType Type => type;

    public enum ShapeType : byte
    {
        ShapeType1,
        ShapeType2,
        ShapeType3,
        ShapeType4,
        ShapeType5,
        ShapeType6,
        ShapeType7,
    }

    public void EnableShape(bool value)
    {
        isEnabled = value;
    }
}