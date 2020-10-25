using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNextPathPoint : Action
{
    public SharedGameObjectList pathPoints;
    public SharedGameObject storeGameObject;
    int index = 0;

    public override TaskStatus OnUpdate()
    {
        storeGameObject.Value = pathPoints.Value[index];
        index = (++index) % pathPoints.Value.Count;

        return TaskStatus.Success;
    }
}