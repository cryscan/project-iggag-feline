using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] GameState startState = GameState.Plan;

    void Start()
    {
        var game = GameManager.instance;
        game.startState = startState;
        if (game.currentState == GameState.Start) game.PushState(startState);
    }
}
