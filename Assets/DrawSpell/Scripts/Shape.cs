using FriendsGamesTools.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Shape : MonoBehaviour
{
    [SerializeField] private ShapeType m_type;
    [SerializeField] private TweenInTime m_tween;

    private bool isEnabled = true;

    public bool IsEnabled => isEnabled;
    public ShapeType Type => m_type;

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

    public void Animate()
    {
        m_tween.SetEnabled(true);
    }
}