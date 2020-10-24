using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [System.Serializable]
    struct Attachment
    {
        public Transform mesh;
        public Transform pivot;
    }

    [Header("Body")]
    [SerializeField] Transform body;
    [SerializeField] Transform bodyPivot;

    [Header("Attachments")]
    [SerializeField] List<Attachment> attachments;

    void LateUpdate()
    {
        body.SetPositionAndRotation(bodyPivot.position, bodyPivot.rotation);
        foreach (var attachment in attachments)
        {
            var pivot = attachment.pivot;
            attachment.mesh.SetPositionAndRotation(pivot.position, pivot.rotation);
        }
    }
}
