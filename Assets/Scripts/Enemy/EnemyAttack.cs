using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // GameManager.instance.GameOverReturn();
            Scene scene = SceneManager.GetActiveScene();
	        GameManager.instance.EnterPlanScene(scene.name);
        }
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
