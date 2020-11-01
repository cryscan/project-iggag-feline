using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    CharacterController controller;
    Vector3 displacement;

    Subscription<HideEvent> hideHandler;
    Subscription<ComeOutEvent> comeOutHandler;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnHidden(HideEvent @event)
    {
        if (@event.subject.gameObject != gameObject) return;

        displacement = @event.displacement;

        controller.enabled = false;
        transform.position = new Vector3(100, 100, 100);

        @event._object.virtualCamera.Priority = 20;
    }

    void OnComeOut(ComeOutEvent @event)
    {
        if (@event.subject.gameObject != gameObject) return;

        transform.position = @event._object.transform.position + displacement;
        controller.enabled = true;

        @event._object.virtualCamera.Priority = 0;
    }
}
