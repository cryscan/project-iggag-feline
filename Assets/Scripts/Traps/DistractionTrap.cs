using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Feline.AI.Sensors;

public class DistractionTrap : TrapBase
{
    [SerializeField] float _range = 10;
    public float range { get => _range; }

    [SerializeField] int maxCount = 2;
    List<GuardAlertSensor> sensors;

    public override void Activate()
    {
        sensors = new List<GuardAlertSensor>();
        base.Activate();

        var query = from sensor in sensors
                    let _object = sensor.gameObject
                    orderby Vector3.Distance(_object.transform.position, transform.position)
                    select _object;
        var objects = query.ToList();
        for (int i = 0; i < maxCount; ++i)
        {
            if (i >= objects.Count) break;
            var sensor = objects[i].GetComponent<GuardAlertSensor>();
            sensor?.Distract(transform.position);
        }
    }

    public override float GetRange() => _range;

    public void RegisterSensor(GuardAlertSensor sensor) => sensors.Add(sensor);
}
