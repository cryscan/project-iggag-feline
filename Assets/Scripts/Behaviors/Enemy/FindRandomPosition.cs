using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FindRandomPosition : Action
{
    public SharedVector3 origin;
    public SharedVector3 extend;

    public SharedVector3 storeValue;

    public override TaskStatus OnUpdate()
    {
        float x = origin.Value.x + Random.Range(-extend.Value.x, extend.Value.x);
        float y = origin.Value.y + Random.Range(-extend.Value.y, extend.Value.y);
        float z = origin.Value.z + Random.Range(-extend.Value.z, extend.Value.z);

        storeValue.Value = new Vector3(x, y, z);

        return TaskStatus.Success;
    }
}