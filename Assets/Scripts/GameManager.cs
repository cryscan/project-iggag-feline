using System.Collections;
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

public class GameStateChangeEvent : IEvent
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
    public bool practiceMode = false;

    Stack<GameState> states = new Stack<GameState>();
    public GameState currentState { get => states.Peek(); }

    [SerializeField] float pauseTimeScaleFallout = 10;
    float targetTimeScale = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        states.Push(GameState.Start);
    }

    void Update()
    {
        // UpdatePlanState();

        // if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();

        targetTimeScale = currentState == GameState.Paused ? 0 : 1;
        Time.timeScale = Time.timeScale.FalloutUnscaled(targetTimeScale, pauseTimeScaleFallout);
    }

    /*
        void UpdatePlanState()
        {
            if (currentState != GameState.Plan) return;

            if (planTimer < 0 || Input.GetKeyDown(KeyCode.Return))
            {
                var previous = states.Pop();
                states.Push(GameState.Play);

                // Change from plan to play.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                StartCoroutine(PublishEventCoroutine(previous));
            }

            planTimer -= Time.deltaTime;
        }

        IEnumerator PublishEventCoroutine(GameState previous)
        {
            yield return null;
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }
    */

    public void TogglePause()
    {
        if (currentState == GameState.Paused)
        {
            var previous = states.Pop();
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }
        else if (currentState != GameState.Start)
        {
            var previous = currentState;
            states.Push(GameState.Paused);
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }
    }

    public void PushPauseState()
    {
        var previous = currentState;
        states.Push(GameState.Paused);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
    }

    public void PopPauseState()
    {
        if (currentState != GameState.Paused)
        {
            Debug.LogError("[Game Manager] is not pausing");
            return;
        }

        var previous = states.Pop();
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
    }

    public void GameOverReturn(int index = 0)
    {
        StartCoroutine(LoadSceneCoroutine(index, () =>
        {
            var previous = states.Pop();
            states = new Stack<GameState>();
            states.Push(GameState.Start);
            EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        }));
    }

    public void EnterPlanScene(int index) => StartCoroutine(LoadSceneCoroutine(index, () => StartPlan()));
    public void EnterPlanScene(string name) => StartCoroutine(LoadSceneCoroutine(name, () => StartPlan()));

    public void EnterPlayScene(int index) => StartCoroutine(LoadSceneCoroutine(index, () => StartPlay()));
    public void EnterPlayScene(string name) => StartCoroutine(LoadSceneCoroutine(name, () => StartPlay()));

    public void EnterPlanSceneRelocate(string name)
    {
        var player = GameObject.FindWithTag("Player");
        var position = player.transform.position;
        var rotation = player.transform.rotation;

        StartCoroutine(LoadSceneCoroutine(name, () =>
        {
            StartPlan();

            player = GameObject.FindWithTag("Player");
            var controller = player.GetComponent<CharacterController>();
            var _enabled = controller.enabled;
            controller.enabled = false;
            player.transform.SetPositionAndRotation(position, rotation);
            controller.enabled = _enabled;
        }));
    }

    public void EnterPlaySceneRelocate(string name)
    {
        var player = GameObject.FindWithTag("Player");
        var position = player.transform.position;
        var rotation = player.transform.rotation;

        StartCoroutine(LoadSceneCoroutine(name, () =>
        {
            StartPlay();

            player = GameObject.FindWithTag("Player");
            var controller = player.GetComponent<CharacterController>();
            var _enabled = controller.enabled;
            controller.enabled = false;
            player.transform.SetPositionAndRotation(position, rotation);
            controller.enabled = _enabled;
        }));
    }

    public void StartPlan()
    {
        if (currentState == GameState.Paused)
        {
            Debug.LogError("[Game Manager] start plan at paused");
            return;
        }

        var previous = currentState;
        states.Push(GameState.Plan);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
    }

    public void StartPlay()
    {
        if (currentState == GameState.Paused)
        {
            Debug.LogError("[Game Manager] start play at paused");
            return;
        }

        var previous = currentState;
        states.Push(GameState.Play);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
    }

    IEnumerator LoadSceneCoroutine(int index, System.Action callback = null)
    {
        SceneManager.LoadScene(index);
        yield return null;
        callback?.Invoke();
    }

    IEnumerator LoadSceneCoroutine(string name, System.Action callback = null)
    {
        SceneManager.LoadScene(name);
        yield return null;
        callback?.Invoke();
    }
}
