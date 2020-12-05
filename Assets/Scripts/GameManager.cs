﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BehaviorDesigner.Runtime;

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

    public GameState startState;

    Stack<GameState> states = new Stack<GameState>();
    public GameState currentState { get => states.Peek(); }

    public bool transiting { get; private set; } = false;

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

    public void PushState(GameState state)
    {
        var previous = currentState;
        states.Push(state);
        EventBus.Publish(new GameStateChangeEvent(previous, currentState));
        Debug.Log($"[Game Manager] pushed state {currentState} onto {previous}");
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
        Debug.Log($"[Game Manager] popped state {previous}, now {currentState}");
    }

    public void GameOverReturn(int index = 0, bool fade = false) => StartCoroutine(LoadSceneCoroutine(index, GameState.Start, null, fade));

    public void EnterPlanScene(int index, bool fade = false) => StartCoroutine(LoadSceneCoroutine(index, GameState.Plan, null, fade));
    public void EnterPlanScene(string name, bool fade = false) => StartCoroutine(LoadSceneCoroutine(name, GameState.Plan, null, fade));

    public void EnterPlayScene(int index, bool fade = false) => StartCoroutine(LoadSceneCoroutine(index, GameState.Play, null, fade));
    public void EnterPlayScene(string name, bool fade = false) => StartCoroutine(LoadSceneCoroutine(name, GameState.Play, null, fade));

    public void RestartCurrentScene()
    {
        if (transiting) return;

        var index = SceneManager.GetActiveScene().buildIndex;

        states = new Stack<GameState>();
        states.Push(GameState.Start);
        StartCoroutine(LoadSceneCoroutine(index, null, null, true));

        GlobalVariables.Instance.GetVariable("Restarted").SetValue(true);
    }

    public void EnterSceneRelocate(string name, GameState? state = null, bool fade = false)
    {
        if (transiting) return;

        var player = GameObject.FindWithTag("Player");
        var position = player.transform.position;
        var rotation = player.transform.rotation;

        System.Action callback = () =>
        {
            player = GameObject.FindWithTag("Player");
            var controller = player.GetComponent<CharacterController>();
            var _enabled = controller.enabled;

            controller.enabled = false;
            player.transform.SetPositionAndRotation(position, rotation);
            controller.enabled = _enabled;
        };

        StartCoroutine(LoadSceneCoroutine(name, state, callback, fade));
    }

    IEnumerator LoadSceneCoroutine(int index, GameState? state = null, System.Action callback = null, bool fade = false)
    {
        transiting = true;

        if (fade)
        {
            EventBus.Publish(new FadeOutEvent());
            yield return new WaitForSeconds(2);
        }

        SceneManager.LoadScene(index);
        if (state.HasValue) PushState(state.Value);

        yield return null;
        callback?.Invoke();

        transiting = false;
    }

    IEnumerator LoadSceneCoroutine(string name, GameState? state = null, System.Action callback = null, bool fade = false)
    {
        transiting = true;

        if (fade)
        {
            EventBus.Publish(new FadeOutEvent());
            yield return new WaitForSeconds(2);
        }

        SceneManager.LoadScene(name);
        if (state.HasValue) PushState(state.Value);

        yield return null;
        callback?.Invoke();

        transiting = false;
    }
}
