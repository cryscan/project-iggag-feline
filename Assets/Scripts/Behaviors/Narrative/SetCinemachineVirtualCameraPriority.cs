using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using Cinemachine;

public class SetCinemachineVirtualCameraPriority : Action
{
    public SharedGameObject target;
    public SharedInt priority;

    CinemachineVirtualCamera virtualCamera;

    public override void OnStart()
    {
        if (target.Value == null) virtualCamera = GetComponent<CinemachineVirtualCamera>();
        else virtualCamera = target.Value.GetComponent<CinemachineVirtualCamera>();
    }

    public override TaskStatus OnUpdate()
    {
        virtualCamera.Priority = priority.Value;
        return TaskStatus.Success;
    }
}