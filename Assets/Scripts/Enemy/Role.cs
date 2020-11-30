using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour
{
    [SerializeField] GameObject _reservation;
    public GameObject reservation { get => _reservation; }

    void OnDisable()
    {
        if (reservation != null) Release(_reservation);
    }

    public bool IsAvailable() => enabled && reservation == null;
    public bool IsReserved(GameObject _object) => reservation == _object;

    public bool Reserve(GameObject _object)
    {
        if (IsAvailable())
        {
            _reservation = _object;
            return true;
        }
        else if (IsReserved(_object)) return true;

        if (enabled) Debug.LogWarning($"[Role] {ToString()} has been reserved by {this.reservation}, can't be reserved by {_object}");
        else Debug.LogWarning($"[Role] {ToString()} cannot reserve: not valid");

        return false;
    }

    public void Release(GameObject _object)
    {
        if (IsReserved(_object)) _reservation = null;
    }
}
