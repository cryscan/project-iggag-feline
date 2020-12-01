using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveOnGameState : MonoBehaviour
{
    [SerializeField] GameState[] states;
    [SerializeField] GameObject[] objects;

    void Update()
    {
        var state = GameManager.instance.currentState;
        var active = states.Contains(state);
        foreach (var _object in objects) _object.SetActive(active);
    }
}
