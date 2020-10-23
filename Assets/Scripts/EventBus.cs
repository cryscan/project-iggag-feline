#define EVENTBUS_DEBUG

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class EventBus
{
    static Dictionary<Type, IList> topics = new Dictionary<Type, IList>();

    public static void Publish<T>(T publishedEvent)
    {
        /* Use type T to identify correct subscriber list (correct "topic") */
        Type t = typeof(T);

#if EVENTBUS_DEBUG
        Debug.Log("[Publish] event of type " + t + " with contents (" + publishedEvent.ToString() + ")");
#endif

        if (topics.ContainsKey(t))
        {
            IList subscribers = new List<Subscription<T>>(topics[t].Cast<Subscription<T>>());

            /* iterate through the subscribers and pass along the event T */
#if EVENTBUS_DEBUG
            Debug.Log("..." + subscribers.Count + " subscriptions being executed for this event.");
#endif

            /* This is a collection of subscriptions that have lost their target object. */
            List<Subscription<T>> orphanedSubscriptions = new List<Subscription<T>>();

            foreach (Subscription<T> s in subscribers)
            {
                if (s.callback.Target == null || s.callback.Target.Equals(null))
                {
                    /* This callback is hanging, as its target object was destroyed */
                    /* Collect this callback and remove it later */
                    orphanedSubscriptions.Add(s);

                }
                else
                {
                    s.callback(publishedEvent);
                }
            }

            /* Unsubcribe orphaned subs that have had their target objects destroyed */
            foreach (Subscription<T> subscription in orphanedSubscriptions)
            {
                EventBus.Unsubscribe<T>(subscription);
            }

        }
        else
        {
#if EVENTBUS_DEBUG
            Debug.Log("...but no one is subscribed to this event right now.");
#endif
        }
    }

    public static Subscription<T> Subscribe<T>(Action<T> callback)
    {
        /* Determine event type so we can find the correct subscriber list */
        Type t = typeof(T);
        Subscription<T> subscription = new Subscription<T>(callback);

        /* If a subscriber list doesn't exist for this event type, create one */
        if (!topics.ContainsKey(t))
            topics[t] = new List<Subscription<T>>();

        topics[t].Add(subscription);

#if EVENTBUS_DEBUG
        Debug.Log("[Subscribe] subscription of function (" + callback.Target.ToString() + "." + callback.Method.Name + ") to type " + t + ". There are now " + topics[t].Count + " subscriptions to this type.");
#endif

        return subscription;
    }

    public static void Unsubscribe<T>(Subscription<T> subscription)
    {
        Type t = typeof(T);

#if EVENTBUS_DEBUG
        Debug.Log("[Unsubscribe] attempting to remove subscription to type " + t);
#endif

        if (topics.ContainsKey(t) && topics[t].Count > 0)
        {
            topics[t].Remove(subscription);

#if EVENTBUS_DEBUG
            Debug.Log("...there are now " + topics[t].Count + " subscriptions to this type.");
#endif
        }
        else
        {
#if EVENTBUS_DEBUG
            Debug.Log("...but this subscription is not currently valid (perhaps you already unsubscribed?)");
#endif
        }
    }
}


/* A "handle" type that is returned when the EventBus.Subscribe() function is used.
 * Use this handle to unsubscribe if you wish via EventBus.Unsubscribe */
public class Subscription<T>
{
    public Action<T> callback { get; private set; }
    public Subscription(Action<T> _callback)
    {
        callback = _callback;
    }

    ~Subscription()
    {
        EventBus.Unsubscribe<T>(this);
    }
}