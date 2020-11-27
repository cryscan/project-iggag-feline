using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform positionTarget;
    [SerializeField] Transform rotationTarget;
    [SerializeField] bool look = false;

    void Awake()
    {
        transform.parent = null;
        if (look) rotationTarget = GameObject.FindWithTag("Player Head").transform;
    }

    void Update()
    {
        if (positionTarget) transform.position = positionTarget.position;
        if (rotationTarget) transform.rotation = rotationTarget.rotation;
    }
}
