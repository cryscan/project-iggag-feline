using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetClosestPathPoints : Action
{
    public SharedGameObjectList pathPoints;
    public SharedVector3 referencePoint;

    public SharedGameObjectList storePathPoints;

    GameObject FindClosestPoint(List<GameObject> points, Vector3 refPoint, out int index)
    {
        GameObject result = null;
        float min = float.PositiveInfinity;

        index = 0;
        for (int i = 0; i < points.Count; ++i)
        {
            var point = points[i];
            var distance = Vector3.Distance(refPoint, point.transform.position);
            if (distance < min)
            {
                result = point;
                distance = min;
                index = i;
            }
        }

        return result;
    }

    public override TaskStatus OnUpdate()
    {
        if (pathPoints.Value.Count == 0) return TaskStatus.Failure;

        List<GameObject> points = new List<GameObject>(pathPoints.Value);
        storePathPoints.Value = new List<GameObject>();

        Vector3 refPoint = referencePoint.Value;
        while (points.Count > 0)
        {
            int index;
            var point = FindClosestPoint(points, refPoint, out index);
            storePathPoints.Value.Add(point);
            refPoint = point.transform.position;
            points.RemoveAt(index);
        }

        return TaskStatus.Success;
    }
}