using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] bool play = false;

    void Start()
    {
        StartCoroutine(StarterCoroutine());
    }

    IEnumerator StarterCoroutine()
    {
        yield return null;

        if (GameManager.instance.currentState == GameState.Start)
        {
            if (!play) GameManager.instance.StartPlan();
            else GameManager.instance.StartPlay();
        }
    }
}
