using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameState startState = GameState.Plan;

    void Start()
    {
        StartCoroutine(StarterCoroutine());
    }

    IEnumerator StarterCoroutine()
    {
        yield return null;

        if (GameManager.instance.currentState == GameState.Start)
        {
            if (startState == GameState.Plan) GameManager.instance.StartPlan();
            else if (startState == GameState.Play) GameManager.instance.StartPlay();
            else Debug.LogError($"[Game Starter] unsupported start state {startState}");
        }

        GameManager.instance.startState = startState;
    }
}
