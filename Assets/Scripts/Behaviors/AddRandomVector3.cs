using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AddRandomVector3 : Action
{
    public SharedVector3 vector;
    public SharedVector3 extend;

    public SharedVector3 storeVector;

    public override TaskStatus OnUpdate()
    {
        float x = vector.Value.x + Random.Range(-extend.Value.x, extend.Value.x);
        float y = vector.Value.y + Random.Range(-extend.Value.y, extend.Value.y);
        float z = vector.Value.z + Random.Range(-extend.Value.z, extend.Value.z);
        storeVector.Value = new Vector3(x, y, z);
        return TaskStatus.Success;
    }
}