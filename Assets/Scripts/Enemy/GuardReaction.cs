using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(ConeDetection))]
public class GuardReaction : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] Color[] colors = { Color.white, Color.yellow, Color.red };
    [SerializeField] float alertSpeed = 8;
    [SerializeField] float dealertSpeed = 2;

    BehaviorTree behavior;
    LineRenderer lineRenderer;

    GameObject player;
    PlayerVisibility visibility;

    float alertLevel = 0;
    bool detected = false;
    bool alerted = false;

    Subscription<DetectEvent> detectEventHandler;
    Subscription<LossTargetEvent> lossTargetHandler;
    Subscription<TrapEvent> trapHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        lineRenderer = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
        visibility = player.GetComponent<PlayerVisibility>();
    }

    void OnEnable()
    {
        detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
        lossTargetHandler = EventBus.Subscribe<LossTargetEvent>(OnLostTarget);
        trapHandler = EventBus.Subscribe<TrapEvent>(OnTrapped);
        behavior.RegisterEvent("Dealert", OnDealerted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(detectEventHandler);
        EventBus.Unsubscribe(lossTargetHandler);
        EventBus.Unsubscribe(trapHandler);
        behavior.UnregisterEvent("Dealert", OnDealerted);
    }

    /*
    void Update()
    {
        if (coneDetection.detected) alertProgress += alertSpeed * visibility.visibility * Time.deltaTime;
        else alertProgress -= dealertSpeed * Time.deltaTime;

        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (alertProgress > distance)
        {
            Alert();
            if (alertLevel < 2) alertProgress = 0;
            else alertProgress = distance;
        }
        else if (alertProgress < 0)
        {
            if (alertLevel > 0) alertProgress = distance;
            else alertProgress = 0;
            Dealert();
        }

        lineRenderer.SetPosition(0, transform.position);
        var direction = player.transform.position - transform.position;
        lineRenderer.SetPosition(1, transform.position + direction.normalized * alertProgress);
        lineRenderer.startColor = lineRenderer.endColor = colors[alertLevel];
    }
    */

    void OnDrawGizmos()
    {
        if (player)
        {
            var direction = player.transform.position - transform.position;
            direction.Normalize();
            Debug.DrawRay(transform.position, direction * alertLevel, Color.red, 0);
        }
    }

    void Update()
    {
        if (detected) alertLevel += alertSpeed * visibility.visibility * Time.deltaTime;
        else alertLevel -= dealertSpeed * Time.deltaTime;

        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (alertLevel > distance)
        {
            if (!alerted)
            {
                _light.color = colors[2];
                behavior.SetVariableValue("Detected", true);
                alerted = true;
            }
            alertLevel = distance;
        }
        else if (alertLevel < 0) alertLevel = 0;

        lineRenderer.SetPosition(0, transform.position);
        var direction = player.transform.position - transform.position;
        lineRenderer.SetPosition(1, transform.position + direction.normalized * alertLevel);
        lineRenderer.startColor = lineRenderer.endColor = Color.red;
    }

    void OnDetected(DetectEvent @event)
    {
        if (@event.type == DetectionType.Guard && @event.subject != gameObject) return;

        // _light.color = colors[2];
        // behavior.SetVariableValue("Detected", true);
        detected = true;
    }

    void OnLostTarget(LossTargetEvent @event)
    {
        if (@event.subject != gameObject) return;

        if (alerted)
        {
            _light.color = colors[1];
            behavior.SetVariableValue("Detected", false);
            behavior.SetVariableValue("Spot Point", @event.spotPoint);
            alerted = false;
        }
        detected = false;
    }

    void OnDealerted() => Dealert();

    /*
    public void Alert()
    {
        if (alertLevel < 2)
        {
            ++alertLevel;
            _light.color = colors[alertLevel];
            behavior.SetVariableValue("Alert Level", alertLevel);
        }
    }
    */

    void Dealert()
    {
        /*
        if (alertLevel > 0)
        {
            --alertLevel;
            _light.color = colors[alertLevel];
            behavior.SetVariableValue("Alert Level", alertLevel);
        }
        */
        _light.color = colors[0];
    }

    void OnTrapped(TrapEvent @event)
    {
        var position = @event.trap.transform.position;
        switch (@event.type)
        {
            case TrapType.Frozen:
                TrapHandler.FrozenData data = (TrapHandler.FrozenData)@event.data;
                var distance = Vector3.Distance(position, transform.position);
                if (distance < data.range)
                    behavior.SendEvent<object>("Frozen", data.duration);

                break;
            case TrapType.Distraction:
                _light.color = colors[1];
                behavior.SetVariableValue("Alerted", true);
                behavior.SetVariableValue("Spot Point", position);
                break;
        }
    }
}