using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wand : MonoBehaviour
{
    [SerializeField] private Transform spellSpawnPos;
    private bool isDrawing;

    public bool IsDrawing
    {
        get { return isDrawing; }
        set { isDrawing = value; }
    }

    public Transform SpellSpawnPos
    {
        get { return spellSpawnPos; }
    }
}


