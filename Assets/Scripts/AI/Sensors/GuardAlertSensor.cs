using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    [RequireComponent(typeof(BehaviorTree))]
    public class GuardAlertSensor : ReGoapSensor<string, object>
    {
        [SerializeField] float alertSpeed = 6, dealertSpeed = 2;

        [Tooltip("Time for the agent to give up searching")]
        [SerializeField] float dealertTime = 10;

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


        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            behavior = GetComponent<BehaviorTree>();
        }

        void OnEnable()
        {
            detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
            lossTargetHandler = EventBus.Subscribe<LossTargetEvent>(OnLostTarget);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe(detectEventHandler);
            EventBus.Unsubscribe(lossTargetHandler);
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
            if (detected) alertLevel += alertSpeed * Time.deltaTime;
            else alertLevel -= dealertSpeed * Time.deltaTime;

            var distance = Vector3.Distance(transform.position, player.transform.position);
            alertLevel = Mathf.Clamp(alertLevel, 0, distance + 1);

            if (alertLevel > distance && detected)
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

        IEnumerator DealertCoroutine()
        {
            yield return new WaitForSeconds(dealertTime);
            alerted = false;
            dealertCoroutine = null;
        }
    }
}