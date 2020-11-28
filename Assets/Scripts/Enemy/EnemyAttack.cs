using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject view;
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
            /*
            Scene scene = SceneManager.GetActiveScene();
            if (plan) GameManager.instance.EnterPlanScene(scene.name);
            else GameManager.instance.EnterPlayScene(scene.name);
            */
            Instantiate(prefab, transform.position, transform.rotation);

            collision.gameObject.SetActive(false);
            Destroy(view);
            Destroy(gameObject);
        }
    }

    public void FadeOut()
    {
        EventBus.Publish(new FadeOutEvent());
    }

    public void GameOver()
    {
        GameManager.instance.RestartCurrentScene();
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
