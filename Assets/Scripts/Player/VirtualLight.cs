using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualLight : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float range;

    public virtual float IntensityAtDistance(float distance)
    {
        if (distance <= range) return intensity / (1 + distance * distance);
        else return 0;
    }

    public virtual float IntensityAtPoint(Vector3 point)
    {
        var distance = Vector3.Distance(point, transform.position);
        return IntensityAtDistance(distance);
    }
}
