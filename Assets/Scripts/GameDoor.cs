using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDoor : MonoBehaviour
{
    [SerializeField] float waitTime = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine()
    {
        GameManager.instance.TogglePause();
        EventBus.Publish(new GameWinEvent());

        yield return new WaitForSecondsRealtime(waitTime);

        GameManager.instance.TogglePause();

        Time.timeScale = 1;

        //Heist scene logic
        int index = 0;
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        string end = scene.path.Split('/')[2];
        Debug.Log(end);
        int nextIndex = scene.buildIndex + 1;
        if (end != "End") {
            index = scene.buildIndex + 1;
        }
        GameManager.instance.GameOverReturn(index);
    }
}
