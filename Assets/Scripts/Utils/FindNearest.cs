using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearest : MonoBehaviour
{
    public static GameObject FindNearestGameObject(List<GameObject> objects, Vector3 reference, out int index)
    {
        GameObject result = null;
        float min = float.PositiveInfinity;

        index = 0;
        for (int i = 0; i < objects.Count; ++i)
        {
            var point = objects[i];
            var distance = Vector3.Distance(reference, point.transform.position);
            if (distance < min)
            {
                result = point;
                min = distance;
                index = i;
            }
        }

        return result;
    }
}
