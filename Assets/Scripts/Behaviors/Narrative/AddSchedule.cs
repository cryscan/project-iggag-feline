using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AddSchedule : Action
{
    public SharedFloat timer;
    public SharedGameObject prefab;
    public SharedVector3 position;
    public SharedBool planning;

    public override TaskStatus OnUpdate()
    {
        ScheduleManager.instance.AddSchedule(timer.Value, prefab.Value, position.Value, planning.Value);
        return TaskStatus.Success;
    }
}