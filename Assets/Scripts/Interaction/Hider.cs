using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hider : MonoBehaviour, Interactor
{
    public Hideable hiding { get; private set; }

    public InteractionType[] GetInteractions(Interactable interactable)
    {
        List<InteractionType> interactions = new List<InteractionType>();

        var hidable = interactable?.GetComponent<Hideable>();
        if (hidable)
        {
            if (hiding == null) interactions.Add(InteractionType.Hide);
            else if (hiding == hidable) interactions.Add(InteractionType.ComeOut);
        }

        return interactions.ToArray();
    }

    public void Interact(Interactable interactable, InteractionType type)
    {
        if (!interactable)
        {
            if (type == InteractionType.ComeOut) ComeOut();
            return;
        }

        Hideable hideable = interactable.GetComponent<Hideable>();
        if (!hideable) return;

        var interactions = GetInteractions(interactable);
        if (!interactions.Contains(type)) return;

        if (type == InteractionType.Hide) Hide(hideable);
        else if (type == InteractionType.ComeOut) ComeOut();
    }

    public virtual void Hide(Hideable hideable)
    {
        EventBus.Publish(new HideEvent(this, hideable));
        hiding = hideable;
    }

    public virtual void ComeOut()
    {
        if (hiding == null)
        {
            Debug.LogError("[Hider] cannot come out since not hiding");
            return;
        }
        EventBus.Publish(new ComeOutEvent(this, hiding));
        hiding = null;
    }
}

public class HideEvent
{
    public Hider subject;
    public Hideable _object;
    public Vector3 displacement;

    public HideEvent(Hider subject, Hideable _object)
    {
        this.subject = subject;
        this._object = _object;
        displacement = _object.transform.position - subject.transform.position;
    }
}

public class ComeOutEvent
{
    public Hider subject;
    public Hideable _object;

    public ComeOutEvent(Hider subject, Hideable _object)
    {
        this.subject = subject;
        this._object = _object;
    }
}