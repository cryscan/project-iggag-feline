using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetRangeIndicator : Action
{
    public RangeIndicator indicator;

    public SharedVector3 center;
    public SharedInt radius;

    public override TaskStatus OnUpdate()
    {
        if (center != null) indicator.center = center.Value;
        if (radius != null) indicator.radius = radius.Value;

        return TaskStatus.Success;
    }
}