using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Plan,
    Play,
    Paused,
}

public class StateChangeEvent
{
    public GameState previous;
    public GameState current;

    public StateChangeEvent(GameState previous, GameState current)
    {
        this.previous = previous;
        this.current = current;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    Stack<GameState> states = new Stack<GameState>();
    public GameState currentState { get => states.Peek(); }

    [SerializeField] float _planTimer = 30;
    public float planTimer { get; private set; }

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
    }

    void Update()
    {
        UpdatePlanState();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                targetTimeScale = previousTimeScale;

                var previous = states.Pop();
                EventBus.Publish(new StateChangeEvent(previous, currentState));
            }
            else if (currentState != GameState.Start)
            {
                previousTimeScale = Time.timeScale;
                targetTimeScale = 0;

                var previous = currentState;
                states.Push(GameState.Paused);
                EventBus.Publish(new StateChangeEvent(previous, currentState));
            }
        }

        /* Continuous Pause Behavior */
        Time.timeScale = Time.timeScale.FalloutUnscaled(targetTimeScale, pauseTimeScaleFallout);
    }

    void UpdatePlanState()
    {
        if (currentState != GameState.Plan) return;

        if (planTimer < 0)
        {
            var previous = states.Pop();
            states.Push(GameState.Play);

            // Change from plan to play.
            EventBus.Publish(new StateChangeEvent(previous, currentState));
            planTimer = _planTimer;
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

        var previous = currentState;
        states.Push(GameState.Plan);
        EventBus.Publish(new StateChangeEvent(previous, currentState));
    }

    public void GameOverReturn()
    {
        if (currentState == GameState.Start || currentState == GameState.Paused)
        {
            Debug.LogError($"[Game State] makes no sense game over at state {currentState.ToString()}");
            return;
        }

        var previous = states.Pop();
        EventBus.Publish(new StateChangeEvent(previous, currentState));

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
