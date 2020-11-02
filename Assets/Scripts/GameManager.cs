﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Plan,
    Play,
    Paused,
}

public class GameStateChangeEvent
{
    public GameState previous;
    public GameState current;

    public GameStateChangeEvent(GameState previous, GameState current)
    {
        this.previous = previous;
        this.current = current;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Text memoText;
    public Text timerText;

    Stack<GameState> states = new Stack<GameState>();
    public GameState currentState { get => states.Peek(); }

    [SerializeField] float _planTimer = 30;
    public float planTimer { get; private set; }

    [SerializeField] int _trapCounter = 5;
    public int trapCounter { get; private set; }

    [SerializeField] float pauseTimeScaleFallout = 10;
    float previousTimeScale = 1;
    float targetTimeScale = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }


        states.Push(GameState.Start);
        planTimer = _planTimer;
        trapCounter = _trapCounter;
    }

    void Update()
    {
        UpdatePlanState();

        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();

        /* Continuous Pause Behavior */
        Time.timeScale = Time.timeScale.FalloutUnscaled(targetTimeScale, pauseTimeScaleFallout);
    }

    void UpdatePlanState()
    {
        if (currentState != GameState.Plan) return;

        if (planTimer < 0 || Input.GetKeyDown(KeyCode.Return))
        {
            var previous = states.Pop();
            states.Push(GameState.Play);

            // Change from plan to play.
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }

        planTimer -= Time.deltaTime;
        ShowMemo();       
        ShowTime();
    }

    void ShowMemo() 
    {
        memoText.text = "Memo:";
        foreach (var schedule in ScheduleManager.instance.schedules)
        {
            memoText.text = memoText.text + $"\n{schedule.prefab.GetComponent<TrapHandler>().type}{ schedule.timer}";
        }
    }

    void ShowTime()
    {
        Debug.Log("[Timer]");
        timerText.text = $"Timer: {(_planTimer - planTimer).ToString("0.0")}";
    }

    public void StartGame()
    {
        if (currentState != GameState.Start)
        {
            Debug.LogError($"[Game State] makes no sense starting game at state {currentState.ToString()}");
            return;
        }

        var previous = currentState;
        states.Push(GameState.Plan);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
    }

    public void TogglePause()
    {
        if (currentState == GameState.Paused)
        {
            targetTimeScale = previousTimeScale;

            var previous = states.Pop();
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }
        else if (currentState != GameState.Start)
        {
            previousTimeScale = Time.timeScale;
            targetTimeScale = 0;

            var previous = currentState;
            states.Push(GameState.Paused);
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }
    }

    public void GameOverReturn()
    {
        if (currentState == GameState.Start || currentState == GameState.Paused)
        {
            Debug.LogError($"[Game State] makes no sense game over at state {currentState.ToString()}");
            return;
        }

        var previous = states.Pop();
        states = new Stack<GameState>();
        states.Push(GameState.Start);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));

        planTimer = _planTimer;
        trapCounter = _trapCounter;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ConsumeTrap(int amount = 1) => trapCounter -= amount;
}
