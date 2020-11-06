using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [SerializeField] string playSceneName;

    [SerializeField] int _maxPlanTime = 60;
    public int maxPlanTime { get => _maxPlanTime; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.instance.currentState == GameState.Plan)
        {
            GameManager.instance.EnterPlayScene(playSceneName);
        }
    }
}
