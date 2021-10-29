using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform character;
    private int frameTimeMultiplier = 3;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - character.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, character.position + offset, Time.deltaTime * frameTimeMultiplier);
    }
}
