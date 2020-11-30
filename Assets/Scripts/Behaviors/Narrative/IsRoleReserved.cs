using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsRoleReserved : Conditional
{
    public SharedGameObject target;

    Role role;

    public override void OnStart()
    {
        if (target.Value == null) role = GetComponent<Role>();
        else role = target.Value.GetComponent<Role>();
    }

    public override TaskStatus OnUpdate()
    {
        return (role.reservation != null) ? TaskStatus.Success : TaskStatus.Failure;
    }
}