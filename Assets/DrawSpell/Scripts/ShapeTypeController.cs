using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTypeController : MonoBehaviour
{
    [SerializeField] private ShapeType type;

    public ShapeType Type => type;

    public enum ShapeType : byte
    {
        ShapeType1,
        ShapeType2,
        ShapeType3,
        ShapeType4,
        ShapeType5
    }
}