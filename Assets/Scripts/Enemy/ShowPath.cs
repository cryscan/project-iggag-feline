using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class ShowPath : MonoBehaviour
{
    LineRenderer lineRenderer;
    NavMeshAgent agent;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();

        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameState.Plan)
        {
            lineRenderer.enabled = true;

            var corners = agent.path.corners;
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners);
        }
        else lineRenderer.enabled = false;
    }
}
