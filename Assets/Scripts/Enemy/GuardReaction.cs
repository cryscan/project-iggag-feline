using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    bool frozen = false;

    Coroutine frozenCoroutine = null;

    Subscription<DetectEvent> detectEventHandler;
    Subscription<LossTargetEvent> lossTargetHandler;
    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<ConeDetection>();
        attack = GetComponent<EnemyAttack>();

        player = GameObject.FindGameObjectWithTag("Player");
        visibility = player.GetComponent<PlayerVisibility>();

        GlobalVariables.Instance.SetVariableValue("Player", player);
    }

    void OnEnable()
    {
        detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
        lossTargetHandler = EventBus.Subscribe<LossTargetEvent>(OnLostTarget);
        trapActivateHandler = EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
        behavior.RegisterEvent("Dealert", OnDealerted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(detectEventHandler);
        EventBus.Unsubscribe(lossTargetHandler);
        EventBus.Unsubscribe(trapActivateHandler);
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

    void OnTrapActivated(TrapActivateEvent @event)
    {
        var position = @event.trap.transform.position;
        // var distance = Vector3.Distance(position, transform.position);

        NavMeshPath path = new NavMeshPath();
        var hasPath = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);
        if (!hasPath) return;

        float distance = 0;
        for (int i = 0; i < path.corners.Length - 1; ++i)
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);

        if (@event.trap is FrozenTrap)
        {
            FrozenTrap frozen = (FrozenTrap)@event.trap;
            if (distance < frozen.range)
            {
                if (frozenCoroutine != null) StopCoroutine(frozenCoroutine);
                frozenCoroutine = StartCoroutine(FrozenCoroutine(frozen.duration));
            }
        }
        else if (@event.trap is DistractionTrap)
        {
            DistractionTrap distraction = (DistractionTrap)@event.trap;
            if (distance < distraction.range && !distraction.ReachedMaxCount() && !frozen)
            {
                behavior.SetVariableValue("Alerted", true);
                behavior.SetVariableValue("Spot Point", position);
                searching = true;
                distraction.IncreaseCount();
            }
        }
    }

    IEnumerator FrozenCoroutine(float duration)
    {
        frozen = true;
        behavior.DisableBehavior(true);
        agent.isStopped = true;

        detection.enabled = false;
        attack.enabled = false;
        _light.enabled = false;

        yield return new WaitForSeconds(duration);

        frozen = false;
        agent.isStopped = false;
        behavior.EnableBehavior();

        detection.enabled = true;
        attack.enabled = true;
        _light.enabled = true;

        frozenCoroutine = null;
    }
}