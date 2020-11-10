using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SamplePosition : Action
{
    public SharedVector3 source;
    public SharedFloat radius;

    public SharedVector3 storePosition;

    public override TaskStatus OnUpdate()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(source.Value, out hit, radius.Value, NavMesh.AllAreas))
        {
            storePosition.Value = hit.position;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}