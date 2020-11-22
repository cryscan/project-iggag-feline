using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    public class GuardStandPointSensor : ReGoapSensor<string, object>
    {
        StandPoint[] standPoints;

        void Awake()
        {
            standPoints = FindObjectsOfType<StandPoint>();
        }

        public override void UpdateSensor()
        {
            var state = memory.GetWorldState();

            bool hasAvailable = standPoints.Any(x => x.IsAvailable() || x.reservation == gameObject);
            state.Set("Has Available Stand Point", hasAvailable);

            if (hasAvailable)
            {
                var query = from standPoint in standPoints
                            where standPoint.IsAvailable() || standPoint.reservation == gameObject
                            select standPoint.gameObject;

                int index;
                var nearest = FindNearest.FindNearestGameObject(query.ToList(), transform.position, out index);
                state.Set("Nearest Stand Point", standPoints[index]);
            }
        }
    }
}