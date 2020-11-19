using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SamplePosition : Action
{
    public SharedVector3 source;
    public SharedFloat radius;

    public SharedVector3 storeValue;

    public override TaskStatus OnUpdate()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(source.Value, out hit, radius.Value, NavMesh.AllAreas))
        {
            storeValue.Value = hit.position;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}