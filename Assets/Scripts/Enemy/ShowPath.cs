using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class ShowPath : MonoBehaviour
{
    LineRenderer lineRenderer;
    NavMeshAgent agent;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();

        lineRenderer.enabled = false;
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameState.Plan)
        {
            var corners = agent.path.corners;
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners);
        }
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current == GameState.Plan) lineRenderer.enabled = true;
        else lineRenderer.enabled = false;
    }
}
