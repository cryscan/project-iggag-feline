﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    [RequireComponent(typeof(BehaviorTree))]
    public class GuardAlertSensor : ReGoapSensor<string, object>
    {
        [Header("Dealert")]
        [SerializeField] float dealertSpeed = 4;

        [Tooltip("Time for the agent to give up searching")]
        [SerializeField] float dealertTime = 10;

        [Header("Sight")]
        [SerializeField] float sightAlertSpeed = 10;

        [Header("Hearing")]
        [SerializeField] float hearAlertSpeed = 2;
        [SerializeField] float hearRange = 5;

        [Tooltip("Alert level will increase hear alert speed amount if the player is heard with this speed")]
        [SerializeField] float hearNominalSpeed = 4;

        [Header("Light")]
        [SerializeField] Light _light;

        GameObject player;

        bool detected = false;
        bool canSeePlayer = false;
        bool alerted = false;

        float alertLevel = 0;

        Coroutine dealertCoroutine = null;

        BehaviorTree behavior;

        Subscription<DetectEvent> detectEventHandler;
        Subscription<LossTargetEvent> lossTargetHandler;
        Subscription<PlayerStepEvent> playerStepHandler;
        Subscription<TrapActivateEvent> trapActivateHandler;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            behavior = GetComponent<BehaviorTree>();
        }

        void OnEnable()
        {
            detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
            lossTargetHandler = EventBus.Subscribe<LossTargetEvent>(OnLostTarget);
            playerStepHandler = EventBus.Subscribe<PlayerStepEvent>(OnPlayerStep);
            trapActivateHandler = EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe(detectEventHandler);
            EventBus.Unsubscribe(lossTargetHandler);
            EventBus.Unsubscribe(playerStepHandler);
            EventBus.Unsubscribe(trapActivateHandler);
        }

        void Update()
        {
            UpdateAlertLevel();
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();

            state.Set("Alerted", alerted);
            state.Set("Can See Player", canSeePlayer);

            if (canSeePlayer) state.Set("Spotted Position", player.transform.position);

            UpdateLight();
        }

        void UpdateAlertLevel()
        {
            if (detected) alertLevel += sightAlertSpeed * Time.deltaTime;
            else alertLevel -= dealertSpeed * Time.deltaTime;

            var distance = Vector3.Distance(transform.position, player.transform.position);
            alertLevel = Mathf.Clamp(alertLevel, 0, distance + 1);

            if (alertLevel > distance)
            {
                if (dealertCoroutine != null) StopCoroutine(dealertCoroutine);
                alerted = true;
                dealertCoroutine = StartCoroutine(DealertCoroutine());
            }

            canSeePlayer = detected && alerted;
        }

        void UpdateLight()
        {
            if (canSeePlayer) _light.color = Color.red;
            else if (alerted || detected) _light.color = Color.yellow;
            else _light.color = Color.white;
        }

        void OnDrawGizmos()
        {
            if (player)
            {
                var direction = player.transform.position - transform.position;
                Debug.DrawRay(transform.position, direction.normalized * alertLevel);
            }

            Gizmos.DrawWireSphere(transform.position, hearRange);
        }

        void OnDetected(DetectEvent @event)
        {
            if (@event.target != player || @event.subject != gameObject) return;
            detected = true;
        }

        void OnLostTarget(LossTargetEvent @event)
        {
            if (@event.target != player || @event.subject != gameObject) return;
            detected = false;
        }

        void OnPlayerStep(PlayerStepEvent @event)
        {
            var state = memory.GetWorldState();
            var distance = Vector3.Distance(@event.position, transform.position);
            if (distance < hearRange)
            {
                var normalizedSpeed = @event.velocity.magnitude / hearNominalSpeed;
                alertLevel += normalizedSpeed * hearAlertSpeed;
                state.Set("Spotted Position", @event.position);
                Debug.Log("[Guard] heard player");
            }
        }

        void OnTrapActivated(TrapActivateEvent @event) { }

        IEnumerator DealertCoroutine()
        {
            yield return new WaitForSeconds(dealertTime);
            alerted = false;
            dealertCoroutine = null;
        }
    }
}