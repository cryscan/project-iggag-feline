using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AimAt : Action
{
    public SharedGameObject subject;
    public SharedVector3 target;
    public SharedFloat fallout = 20;

    public override void OnStart()
    {
        if (!subject.Value) subject.Value = gameObject;
    }

    public override TaskStatus OnUpdate()
    {
        var direction = target.Value - subject.Value.transform.position;
        direction.y = 0;

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        angle = 0f.Fallout(angle, fallout.Value);
        transform.Rotate(0, angle, 0, Space.Self);

        if (Mathf.Abs(angle) < 0.1) return TaskStatus.Success;
        else return TaskStatus.Running;
    }
}