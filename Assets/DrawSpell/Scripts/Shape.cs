using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private ShapeType type;

    private bool isEnabled = true;

    public bool IsEnabled => isEnabled;
    public ShapeType Type => type;

    public enum ShapeType : byte
    {
        Shape_Vigvam,
        Shape_V,
        Shape_Lightning,
        Shape_Z,
        Shape_L,
        Shape_I,
        Shape_Stroke,
        Shape_Slash,
        Shape_CounterSlash,
    }

    public void EnableShape(bool value)
    {
        isEnabled = value;
    }
}