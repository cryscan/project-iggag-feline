using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float range = 0.6f;
    [SerializeField] float waitTime = 2;

    GameObject player;
    bool triggered = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < range && !triggered)
        {
            StartCoroutine(LoseCoroutine());
            triggered = true;
        }
    }

    IEnumerator LoseCoroutine()
    {
        GameManager.instance.TogglePause();
        EventBus.Publish(new GameLoseEvent());

        yield return new WaitForSecondsRealtime(waitTime);

        GameManager.instance.TogglePause();

        Time.timeScale = 1;
        GameManager.instance.GameOverReturn();
    }
}
