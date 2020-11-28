using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEvent : IEvent { }

public class FadeOutEvent : IEvent { }

public class FadeInOut : MonoBehaviour
{
    public bool fade = false;

    Animator animator;

    Subscription<FadeInEvent> fadeInHandler;
    Subscription<FadeOutEvent> fadeOutHandler;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        fadeInHandler = EventBus.Subscribe<FadeInEvent>(OnFadeIn);
        fadeOutHandler = EventBus.Subscribe<FadeOutEvent>(OnFadeOut);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(fadeInHandler);
        EventBus.Unsubscribe(fadeOutHandler);
    }

    void Update()
    {
        animator.SetBool("Fade", fade);
    }

    void OnFadeIn(FadeInEvent @event) => fade = false;
    void OnFadeOut(FadeOutEvent @event) => fade = true;
}
