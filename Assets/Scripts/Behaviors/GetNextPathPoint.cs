using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNextPathPoint : Action
{
    public SharedGameObjectList pathPoints;
    public SharedGameObject storeGameObject;
    public SharedInt index = 0;

    public override TaskStatus OnUpdate()
    {
        if (pathPoints.Value.Count == 0) return TaskStatus.Failure;

        storeGameObject.Value = pathPoints.Value[index.Value];
        index.Value = (++index.Value) % pathPoints.Value.Count;

        return TaskStatus.Success;
    }
}