using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNextPathPoint : Action
{
    public SharedGameObjectList pathPointList;
    public SharedInt index = 0;

    public SharedGameObject storeValue;

    public override TaskStatus OnUpdate()
    {
        var pathPoints = pathPointList.Value;
        if (pathPoints == null || pathPoints.Count == 0) return TaskStatus.Failure;

        storeValue.Value = pathPoints[index.Value];
        index.Value = (++index.Value) % pathPoints.Count;

        return TaskStatus.Success;
    }
}