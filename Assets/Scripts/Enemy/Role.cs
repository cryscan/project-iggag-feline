using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour, IRole
{
    [SerializeField] bool _valid;
    public bool valid { get => _valid; }

    public GameObject reservation { get; private set; }

    void Update()
    {
        if (!_valid) reservation = null;
    }

    public bool IsAvailable() => _valid && reservation == null;
    public bool IsReserved(GameObject _object) => reservation == _object;
    public void SetValid(bool valid) => _valid = valid;

    public bool Reserve(GameObject _object)
    {
        if (IsAvailable())
        {
            reservation = _object;
            return true;
        }
        else if (IsReserved(_object)) return true;

        if (_valid) Debug.LogWarning($"[Role] {ToString()} has been reserved by {this.reservation}, can't be reserved by {_object}");
        else Debug.LogWarning($"[Role] {ToString()} cannot reserve: not valid");

        return false;
    }
}


public interface IRole
{
    bool IsAvailable();
    bool IsReserved(GameObject _object);
    void SetValid(bool valid);

    bool Reserve(GameObject _object);
}