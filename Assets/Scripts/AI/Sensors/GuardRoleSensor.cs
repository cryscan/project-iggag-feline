using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    public class GuardRoleSensor<T> : ReGoapSensor<string, object> where T : Role
    {
        T[] ts;

        void Awake()
        {
            ts = FindObjectsOfType(typeof(T)) as T[];
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();
            var name = typeof(T).ToString();

            bool hasAvailable = ts.Any(x => x.IsAvailable() || x.IsReserved(gameObject));
            state.Set($"Has Role {name}", hasAvailable);

            if (hasAvailable)
            {
                var query = from x in ts
                            where x.IsAvailable() || x.IsReserved(gameObject)
                            select x.gameObject;

                int index;
                var nearest = FindNearest.FindNearestGameObject(query.ToList(), transform.position, out index);
                state.Set($"Nearest {name}", nearest.GetComponent<T>());
            }
        }
    }
}