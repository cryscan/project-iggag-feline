using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool rotate = false;

    void Update()
    {
        transform.position = target.position;
        if (rotate) transform.rotation = target.rotation;
    }
}
