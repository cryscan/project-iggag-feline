using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardView : PlayerView
{
    [Header("Oscillation")]
    [SerializeField] float amplitude = 0.05f;
    [SerializeField] float period = 0.5f;
    [SerializeField] float fallout = 10;

    float blend = 0;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent)
        {
            var maxSpeed = agent.speed;
            var speed = agent.velocity.magnitude;
            var target = speed / maxSpeed;
            blend = blend.Fallout(target, fallout);
        }
        else blend = blend.Fallout(0, fallout);
    }

    protected override void LateUpdate()
    {
        var bodyOscillation = Oscillation(0.0f);
        var attachmentOscillation = Oscillation(-Mathf.PI / 4);

        var position = bodyPivot.position;
        position.y += bodyOscillation;
        body.SetPositionAndRotation(position, bodyPivot.rotation);

        foreach (var attachment in attachments)
        {
            position = attachment.pivot.position;
            position.y += attachmentOscillation;
            attachment.mesh.SetPositionAndRotation(position, attachment.pivot.rotation);
        }
    }

    float Oscillation(float phase)
    {
        var frequency = 2 * Mathf.PI / period;
        var oscillation = Mathf.Sin(frequency * Time.time + phase);

        return blend * amplitude * oscillation;
    }
}
