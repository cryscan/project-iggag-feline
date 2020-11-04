using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSpotLight : VirtualLight
{
    [SerializeField] float angle;

    public override float IntensityAtPoint(Vector3 point)
    {
        var direction = point - transform.position;
        if (Vector3.Angle(transform.forward, direction) < angle)
        {
            var distance = direction.magnitude;
            return IntensityAtDistance(distance);
        }
        else return 0;
    }
}
