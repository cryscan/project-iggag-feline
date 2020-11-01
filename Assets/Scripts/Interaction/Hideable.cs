using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Interactable))]
public class Hideable : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera virtualCamera { get => virtualCamera; }

    public Hider inside { get; private set; }

    Subscription<HideEvent> hideHandler;
    Subscription<ComeOutEvent> comeOutHandler;

    void Awake()
    {
        var interactable = GetComponent<Interactable>();
        interactable.Rigister(InteractionType.Hide);
        interactable.Rigister(InteractionType.ComeOut);
    }

    void OnEnable()
    {
        hideHandler = EventBus.Subscribe<HideEvent>(OnHidden);
        comeOutHandler = EventBus.Subscribe<ComeOutEvent>(OnComeOut);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(hideHandler);
        EventBus.Unsubscribe(comeOutHandler);
    }

    void OnHidden(HideEvent @event)
    {
        if (@event._object != this) return;
        inside = @event.subject;
    }

    void OnComeOut(ComeOutEvent @event)
    {
        if (@event._object != this) return;
        Debug.Assert(inside == @event.subject);
        inside = null;
    }
}
