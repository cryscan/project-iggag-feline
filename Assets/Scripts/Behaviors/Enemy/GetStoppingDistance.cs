using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetStoppingDistance : Action
{
    public SharedGameObject targetGameObject;
    public SharedFloat storeValue;

    public override TaskStatus OnUpdate()
    {
        GameObject subject;
        if (targetGameObject.Value != null) subject = targetGameObject.Value;
        else subject = gameObject;

        var agent = subject.GetComponent<NavMeshAgent>();
        storeValue.Value = agent.stoppingDistance;

        return TaskStatus.Success;
    }
}