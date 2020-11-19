using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetNearestPathPoint : Action
{
    public SharedGameObjectList pathPointList;
    public SharedVector3 position;

    public SharedGameObject storeGameObject;
    public SharedInt storeIndex;

    public override TaskStatus OnUpdate()
    {
        int index;
        storeGameObject.Value = FindNearest.FindNearestGameObject(pathPointList.Value, position.Value, out index);
        storeIndex.Value = index;

        return TaskStatus.Success;
    }
}