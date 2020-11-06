using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDoor : MonoBehaviour
{
    [SerializeField] float waitTime = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            GameManager.instance.GameOverReturn();
    }

    /*
        IEnumerator WinCoroutine()
        {
            GameManager.instance.TogglePause();
            EventBus.Publish(new GameWinEvent());

            yield return new WaitForSecondsRealtime(waitTime);

            GameManager.instance.TogglePause();

            Time.timeScale = 1;
        }
    */
}
