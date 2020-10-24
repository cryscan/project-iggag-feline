using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Transform body, bodyPivot;
    [SerializeField] Transform head, headPivot;

    void LateUpdate()
    {
        body.SetPositionAndRotation(bodyPivot.position, bodyPivot.rotation);
        head.SetPositionAndRotation(headPivot.position, headPivot.rotation);
    }
}
