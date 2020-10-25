using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    //Subscription<PauseEvent> pause_subscription;
    public float speed = 2.0f;
    public int startDir = 1;
    private float roty;
    private float rotx;
    private float rotz;
    private bool stopped = false;

    private void Start()
    {
        //pause_subscription = EventBus.Subscribe<PauseEvent>(StopMovement);
        rotx = transform.rotation.eulerAngles.x;
        roty = transform.rotation.eulerAngles.y;
        rotz = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (!stopped)
        {
            //Credit to Segy from https://answers.unity.com/questions/1126169/how-to-rotate-object-back-and-forth-from-one-rotat.html
            transform.rotation = Quaternion.Euler(rotx, roty + startDir * (90.0f * Mathf.Sin(Time.time * speed)), rotz);
        }
    }

    /*public void StopMovement(PauseEvent p)
    {
        stopped = p.stop;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PauseEvent>(pause_subscription);
    }*/
}
