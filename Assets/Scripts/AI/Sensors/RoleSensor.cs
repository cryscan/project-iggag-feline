using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    public class RoleSensor<T> : ReGoapSensor<string, object> where T : Role
    {
        [SerializeField] float range = 5;

        T[] ts;

        void Awake()
        {
            ts = FindObjectsOfType(typeof(T)) as T[];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();
            var type = typeof(T).ToString();

            bool hasAvailable = ts.Any(Criteria);
            state.Set($"Has Role {type}", hasAvailable);

            if (hasAvailable)
            {
                var query = from x in ts
                            where Criteria(x)
                            select x.gameObject;
                var list = query.ToList();

                int index;
                var nearest = FindNearest.FindNearestGameObject(list, transform.position, out index);
                state.Set($"Nearest {type}", nearest.GetComponent<T>());
            }
        }

        bool Criteria(T t)
        {
            var distance = Vector3.Distance(t.transform.position, transform.position);
            return (t.IsAvailable() && distance < range) || t.IsReserved(gameObject);
        }
    }
}