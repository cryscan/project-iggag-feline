using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class StartSchedule : Action
{
    public override TaskStatus OnUpdate()
    {
        ScheduleManager.instance.ResetTimer();
        ScheduleManager.instance.DeploySchedule();

        return TaskStatus.Success;
    }
}