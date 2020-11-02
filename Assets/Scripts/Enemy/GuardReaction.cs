using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(ConeDetection))]
public class GuardReaction : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] Color[] colors = { Color.white, Color.yellow, Color.red };
    [SerializeField] float alertSpeed = 4;
    [SerializeField] float dealertSpeed = 1;

    BehaviorTree behavior;
    ConeDetection coneDetection;
    LineRenderer lineRenderer;

    GameObject player;
    PlayerVisibility visibility;

    public int alertLevel { get; private set; } = 0;
    public float alertProgress { get; private set; } = 0;

    Subscription<DetectEvent> detectEventHandler;
    Subscription<LossTargetEvent> lossTargetHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        coneDetection = GetComponent<ConeDetection>();
        lineRenderer = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
        visibility = player.GetComponent<PlayerVisibility>();
    }

    void OnEnable()
    {
        detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
        lossTargetHandler = EventBus.Subscribe<LossTargetEvent>(OnLostTarget);
        behavior.RegisterEvent("Dealert", OnDealerted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(detectEventHandler);
        EventBus.Unsubscribe(lossTargetHandler);
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

    void OnDrawGizmos()
    {
        if (player)
        {
            var direction = player.transform.position - transform.position;
            direction.Normalize();
            Debug.DrawRay(transform.position, direction * alertProgress, colors[alertLevel], 0);
        }
    }
    */

    void OnDetected(DetectEvent @event)
    {
        if (@event.type == DetectionType.Guard && @event.subject != gameObject) return;

        _light.color = colors[2];
        behavior.SetVariableValue("Detected", true);
    }

    void OnLostTarget(LossTargetEvent @event)
    {
        if (@event.subject != gameObject) return;

        _light.color = colors[1];
        behavior.SetVariableValue("Detected", false);
        behavior.SetVariableValue("Spot Point", @event.spotPoint);
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
}