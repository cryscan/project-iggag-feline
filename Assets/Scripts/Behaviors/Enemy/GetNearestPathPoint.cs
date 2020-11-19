using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNearestPathPoint : Action
{
    public SharedGameObjectList pathPointList;
    public SharedVector3 position;

    public SharedGameObject storeGameObject;
    public SharedInt storeIndex;

    public static GameObject FindNearestPoint(List<GameObject> points, Vector3 reference, out int index)
    {
        GameObject result = null;
        float min = float.PositiveInfinity;

        index = 0;
        for (int i = 0; i < points.Count; ++i)
        {
            var point = points[i];
            var distance = Vector3.Distance(reference, point.transform.position);
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
        int index;
        storeGameObject.Value = FindNearestPoint(pathPointList.Value, position.Value, out index);
        storeIndex.Value = index;

        return TaskStatus.Success;
    }
}