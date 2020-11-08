using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetClosestPathRoute : Action
{
    public SharedGameObjectList pathPoints;
    public SharedVector3 position;

    public SharedGameObjectList storePathPoints;

    public override TaskStatus OnUpdate()
    {
        if (pathPoints.Value.Count == 0) return TaskStatus.Failure;

        List<GameObject> points = new List<GameObject>(pathPoints.Value);
        storePathPoints.Value = new List<GameObject>();

        Vector3 pos = position.Value;
        while (points.Count > 0)
        {
            int index;
            var point = GetClosestPathPoint.FindClosestPoint(points, pos, out index);

            storePathPoints.Value.Add(point);
            pos = point.transform.position;
            points.RemoveAt(index);
        }

        return TaskStatus.Success;
    }
}