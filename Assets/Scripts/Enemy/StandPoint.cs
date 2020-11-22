using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ReGoap.Core;

public class StandPoint : MonoBehaviour
{
    [SerializeField] bool _valid = false;
    public bool valid { get => _valid; }

    public GameObject reservation { get; private set; }

    public List<GameObject> points { get; private set; }

    void Awake()
    {
        var query = from i in Enumerable.Range(0, transform.childCount)
                    select transform.GetChild(i).gameObject;
        points = query.ToList();
    }

    void Update()
    {
        if (!_valid) reservation = null;
    }

    public bool IsAvailable() => _valid && reservation == null;

    public void SetValid(bool valid) => _valid = valid;

    public bool Reserve(GameObject _object)
    {
        if (IsAvailable())
        {
            this.reservation = _object;
            return true;
        }
        else if (this.reservation == _object) return true;

        if (_valid) Debug.LogWarning($"[Stand Point] has been reserved by {this.reservation}, can't be reserved by {_object}");
        else Debug.LogWarning($"[Stand Point] cannot reserve: not valid");

        return false;
    }
}
