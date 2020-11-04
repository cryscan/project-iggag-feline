using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;
    bool triggered = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            GameManager.instance.GameOverReturn();
    }

    /*
    IEnumerator LoseCoroutine()
    {
        GameManager.instance.TogglePause();
        EventBus.Publish(new GameLoseEvent());

        yield return new WaitForSecondsRealtime(waitTime);

        GameManager.instance.TogglePause();

        Time.timeScale = 1;
        GameManager.instance.GameOverReturn();
    }
    */
}
