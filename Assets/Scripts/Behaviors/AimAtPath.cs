﻿using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AimAtPath : Action
{
    public SharedGameObject subject;
    public SharedFloat fallout = 20;

    NavMeshAgent agent;
    float timer = 0;

    public override void OnStart()
    {
        if (!subject.Value) subject = gameObject;
        agent = subject.Value.GetComponent<NavMeshAgent>();
        timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 target;
        try { target = agent.path.corners[1]; }
        catch { return TaskStatus.Running; }

        var direction = target - subject.Value.transform.position;
        direction.y = 0;

        var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        angle = 0f.Fallout(angle, fallout.Value);
        transform.Rotate(0, angle, 0, Space.Self);

        if (timer > 5 / fallout.Value) return TaskStatus.Failure;
        timer += Time.deltaTime;

        if (Mathf.Abs(angle) < 0.1) return TaskStatus.Success;
        else return TaskStatus.Running;
    }
}