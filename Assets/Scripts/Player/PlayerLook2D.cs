using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook2D : MonoBehaviour
{
    [SerializeField] Transform look;
    [SerializeField] float fallout = 20;

    void Update()
    {
        var direction = look.position - transform.position;
        direction.y = 0;

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        angle = 0f.Fallout(angle, fallout);
        transform.Rotate(0, angle, 0, Space.Self);
    }
}
