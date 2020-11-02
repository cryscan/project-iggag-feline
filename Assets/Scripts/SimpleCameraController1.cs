using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController1 : MonoBehaviour
{
    public float speed = 0.5f;
    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(horizontal, 0f, vertical) * speed;      
    }
}
