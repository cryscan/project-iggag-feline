using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [System.Serializable]
    public struct Attachment
    {
        public Transform mesh;
        public Transform pivot;
    }

    [Header("Body")]
    [SerializeField] protected Transform body;
    [SerializeField] protected Transform bodyPivot;

    [Header("Attachments")]
    [SerializeField] protected List<Attachment> attachments;

    protected virtual void LateUpdate()
    {
        body.SetPositionAndRotation(bodyPivot.position, bodyPivot.rotation);
        foreach (var attachment in attachments)
        {
            var pivot = attachment.pivot;
            attachment.mesh.SetPositionAndRotation(pivot.position, pivot.rotation);
        }
    }
}
