using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class GuardReaction : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] Color[] colors = { Color.white, Color.yellow, Color.red };
    [SerializeField] float alertSpeed = 16;
    [SerializeField] float searchingAlertSpeed = 32;
    [SerializeField] float dealertSpeed = 2;

    BehaviorTree behavior;
    NavMeshAgent agent;
    ConeDetection detection;
    EnemyAttack attack;

    GameObject player;
    PlayerVisibility visibility;

    float alertLevel = 0;
    bool detected = false;
    bool chasing = false;
    bool searching = false;

    Subscription<DetectEvent> detectEventHandler;
    Subscription<LossTargetEvent> lossTargetHandler;
    Subscription<TrapEvent> trapHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<ConeDetection>();
        attack = GetComponent<EnemyAttack>();

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

    void Update()
    {
        if (detected)
        {
            if (!searching) alertLevel += alertSpeed * visibility.visibility * Time.deltaTime;
            else alertLevel += searchingAlertSpeed * visibility.visibility * Time.deltaTime;
        }
        else alertLevel -= dealertSpeed * Time.deltaTime;

        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (alertLevel > distance + 1) alertLevel = distance + 1;
        if (alertLevel > distance && detected)
        {
            if (!chasing)
            {
                // _light.color = colors[2];
                behavior.SetVariableValue("Detected", true);
                chasing = true;
            }
        }
        if (alertLevel < 0) alertLevel = 0;

        if (!chasing)
        {
            if (detected) _light.color = colors[1];
            else if (searching) _light.color = colors[1];
            else _light.color = colors[0];
        }
        else
        {
            _light.color = colors[2];
        }
    }

    void OnDetected(DetectEvent @event)
    {
        if (@event.target != player) return;
        if (@event.type == DetectionType.Guard && @event.subject != gameObject) return;

        // _light.color = colors[2];
        // behavior.SetVariableValue("Detected", true);
        detected = true;
    }

    void OnLostTarget(LossTargetEvent @event)
    {
        if (@event.target != player || @event.subject != gameObject) return;

        if (chasing)
        {
            // _light.color = colors[1];
            behavior.SetVariableValue("Detected", false);
            behavior.SetVariableValue("Spot Point", @event.spotPoint);
            chasing = false;
            searching = true;
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
        // _light.color = colors[0];
        searching = false;
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
                {
                    // behavior.SendEvent<object>("Frozen", data.duration);
                    StopAllCoroutines();
                    StartCoroutine(FrozenCoroutine(data.duration));
                }

                break;
            case TrapType.Distraction:
                distance = Vector3.Distance(position, transform.position);
                // if (distance < 3)
                {
                    // _light.color = colors[1];
                    behavior.SetVariableValue("Alerted", true);
                    behavior.SetVariableValue("Spot Point", position);
                    searching = true;
                }
                break;
        }
    }

    IEnumerator FrozenCoroutine(float duration)
    {
        // behavior.enabled = false;
        agent.isStopped = true;
        detection.enabled = false;
        attack.enabled = false;
        _light.enabled = false;

        yield return new WaitForSeconds(duration);

        // behavior.enabled = true;
        agent.isStopped = false;
        detection.enabled = true;
        attack.enabled = true;
        _light.enabled = true;
    }
}