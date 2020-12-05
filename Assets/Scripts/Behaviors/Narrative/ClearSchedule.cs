using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ClearSchedule : Action
{
    public override TaskStatus OnUpdate()
    {
        ScheduleManager.instance.ClearSchedule();
        return TaskStatus.Success;
    }
}