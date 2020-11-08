using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hideable : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public GameObject inside { get; private set; }

    void Awake()
    {
        virtualCamera.Priority = 0;
    }

    public void Hide(GameObject subject)
    {
        if (!subject)
        {
            Debug.LogError($"[Hide] no subject");
            return;
        }

        if (subject != inside)
        {
            inside = subject;
            virtualCamera.Priority = 100;
            EventBus.Publish(new HideEvent(subject, this));
        }
        else Debug.LogWarning($"[Hide] {subject} hides again");
    }

    public void ComeOut()
    {
        if (!inside)
        {
            Debug.LogError($"[Hide] nothing is inside");
            return;
        }

        EventBus.Publish(new ComeOutEvent(inside, this));
        inside = null;
        virtualCamera.Priority = 0;
    }
}

public class HideEvent
{
    public GameObject subject;
    public Hideable hideable;

    public HideEvent(GameObject subject, Hideable hideable)
    {
        this.subject = subject;
        this.hideable = hideable;
    }
}

public class ComeOutEvent
{
    public GameObject subject;
    public Hideable hideable;

    public ComeOutEvent(GameObject subject, Hideable hideable)
    {
        this.subject = subject;
        this.hideable = hideable;
    }
}