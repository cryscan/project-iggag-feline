using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class PredictPlayerPosition : Action
{
    public LayerMask blockLayers;
    public SharedVector3 storePosition;

    NavMeshAgent agent;
    CharacterController controller;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    public override TaskStatus OnUpdate()
    {
        var playerVelocity = controller.velocity;
        var speed = agent.speed;



        return TaskStatus.Success;
    }
}