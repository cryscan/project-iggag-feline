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
        {
            EventBus.Publish(new GameWinEvent());
            StartCoroutine(WinCoroutine());
        }
    }

    IEnumerator WinCoroutine()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(waitTime);
        GameManager.instance.GameOverReturn();
        Time.timeScale = 1;
    }
}
