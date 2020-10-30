using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform head;

    [SerializeField] float sensitivity = 1;
    [SerializeField] float fallout = 10;

    float pitch = 0;
    float _horizontal, _vertical;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X") * sensitivity * Time.timeScale;
        float vertical = Input.GetAxis("Mouse Y") * sensitivity * Time.timeScale;

        _horizontal = _horizontal.FalloutUnscaled(horizontal, fallout);
        _vertical = _vertical.FalloutUnscaled(vertical, fallout);

        Debug.Log(_horizontal);

        pitch -= _vertical;
        pitch = Mathf.Clamp(pitch, -85, 85);
        head.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        transform.Rotate(Vector3.up * _horizontal);
    }
}
