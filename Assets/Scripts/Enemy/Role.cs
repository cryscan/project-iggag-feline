using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    public GameObject reservation { get; private set; }

    Subscription<RoleReleaseEvent> roleReleaseHandler;

    void OnEnable()
    {
        roleReleaseHandler = EventBus.Subscribe<RoleReleaseEvent>(OnRoleReleased);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(roleReleaseHandler);
        reservation = null;
    }

    public bool IsAvailable() => enabled && reservation == null;
    public bool IsReserved(GameObject _object) => reservation == _object;

    public bool Reserve(GameObject _object)
    {
        if (IsAvailable())
        {
            reservation = _object;
            return true;
        }
        else if (IsReserved(_object)) return true;

        if (enabled) Debug.LogWarning($"[Role] {ToString()} has been reserved by {this.reservation}, can't be reserved by {_object}");
        else Debug.LogWarning($"[Role] {ToString()} cannot reserve: not valid");

        return false;
    }

    public void Release(GameObject _object)
    {
        if (IsReserved(_object)) reservation = null;
    }

    void OnRoleReleased(RoleReleaseEvent @event) => Release(@event.subject);
}

public class RoleReleaseEvent { public GameObject subject; }