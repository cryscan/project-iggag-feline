using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    [SerializeField] float distance = 10.0f;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotateTime = 1.0f;

    bool stopped = false;
    float timeLeft = 0.0f;

    Rigidbody rb;

    /*
    // Start is called before the first frame update
    void Start()
    {
        //pause_subscription = EventBus.Subscribe<PauseEvent>(StopMovement);
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(speed * horizontalDir, 0.0f, speed * verticalDir);
        timeLeft = distance / speed;
        verticalDir *= -1;
        horizontalDir *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            if (timeLeft > 0.0f)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0.0f && rb.velocity != Vector3.zero)
                {
                    rb.velocity = Vector3.zero;
                    timeLeft = rotateTime;
                    StartCoroutine(RotateGuard());
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.velocity = new Vector3(speed * horizontalDir, 0.0f, speed * verticalDir);
                timeLeft = distance / speed;
                verticalDir *= -1;
                horizontalDir *= -1;
            }
        }
    }

    public IEnumerator RotateGuard()
    {
        float initTime = Time.time;
        float progress = (Time.time - initTime) / rotateTime;
        float degreesPerSec = 180.0f / rotateTime;
        float initRotate = transform.rotation.eulerAngles.y;
        while (progress < 1.0f)
        {
            progress = (Time.time - initTime) / rotateTime;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, initRotate + (Time.time - initTime) * degreesPerSec, 0.0f));

            yield return null;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0.0f, initRotate + 180.0f, 0.0f));

    }

    /*public void StopMovement(PauseEvent p)
    {
        stopped = p.stop;
        rb.velocity = Vector3.zero;
    } 

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PauseEvent>(pause_subscription);
    }*/
}
