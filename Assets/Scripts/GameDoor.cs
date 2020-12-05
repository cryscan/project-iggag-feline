using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDoor : MonoBehaviour
{
    [SerializeField] float waitTime = 2;
    public int nextLevelNum = 0;
    public bool conditionalDoor = false;
    public bool condition = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!conditionalDoor || condition)
            {
                if (!GameManager.instance.practiceMode)
                {
                    // MainMenuController.UnlockLevel(nextLevelNum);
                }

                StartCoroutine(WinCoroutine());
            }
        }

    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSecondsRealtime(waitTime);

        //Heist scene logic
        var nextIndex = GameManager.instance.practiceMode ? 0 : GetNextIndex();
        GameManager.instance.EnterPlanScene(nextIndex);
    }

    int GetNextIndex()
    {
        int index = 0;
        Scene scene = SceneManager.GetActiveScene();

        string end = scene.path.Split('/')[3];
        Debug.Log(end);

        int nextIndex = scene.buildIndex + 1;
        if (end != "End") index = scene.buildIndex + 1;

        return index;
    }
}
