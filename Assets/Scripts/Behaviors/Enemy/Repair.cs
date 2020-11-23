using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Repair : Action
{
    public SharedGameObject target;

    Breakable breakable;

    public override void OnStart()
    {
        breakable = target.Value.GetComponent<Breakable>();
    }

    public override TaskStatus OnUpdate()
    {
        if (breakable.broken)
            breakable.Repair();

        return TaskStatus.Success;
    }
}