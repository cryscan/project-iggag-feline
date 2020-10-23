using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    Plan,
    Play,
    Paused,
}

public class EnterPlayEvent { }

public class PauseEvent { }
public class ResumeEvent { }

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    Stack<GameState> states = new Stack<GameState>();
    public GameState currentState { get => states.Peek(); }

    [SerializeField] float initPlanTimer = 30;
    public float planTimer { get; private set; }

    [SerializeField] float pauseTimeScaleFallout = 10;
    float previousTimeScale = 1;

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
        planTimer = initPlanTimer;

        EventBus.Subscribe<PauseEvent>(OnPaused);
        EventBus.Subscribe<ResumeEvent>(OnResumed);
    }

    void Update()
    {
        UpdatePlanState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                states.Pop();
                EventBus.Publish(new ResumeEvent());
            }
            else if (currentState != GameState.Start)
            {
                states.Push(GameState.Paused);
                EventBus.Publish(new PauseEvent());
            }
        }
    }

    void UpdatePlanState()
    {
        if (currentState != GameState.Plan) return;

        if (planTimer < 0)
        {
            states.Pop();
            states.Push(GameState.Play);
            EventBus.Publish(new EnterPlayEvent());
            planTimer = initPlanTimer;
        }

        planTimer -= Time.deltaTime;
    }

    public void StartGame()
    {
        if (currentState != GameState.Start)
        {
            Debug.LogError($"[Game State] makes no sense starting game at state {currentState.ToString()}");
            return;
        }

        states.Push(GameState.Plan);
    }

    public void GameOverReturn()
    {
        if (currentState == GameState.Start || currentState == GameState.Paused)
        {
            Debug.LogError($"[Game State] makes no sense game over at state {currentState.ToString()}");
            return;
        }

        states.Pop();
    }

    void OnPaused(PauseEvent @event)
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = Time.timeScale.FalloutUnscaled(0, pauseTimeScaleFallout);
    }

    void OnResumed(ResumeEvent @event)
    {
        Time.timeScale = Time.timeScale.FalloutUnscaled(previousTimeScale, pauseTimeScaleFallout);
    }
}
