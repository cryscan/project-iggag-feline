using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    GameManager game;
    public GameObject MainMenu;
    public GameObject HeistMenu;
    public GameObject PracticeMenu;
    // public GameObject Controls;
    public GameObject HeistTutorial;
    public GameObject Heist1;
    public GameObject L0_1;
    public GameObject L0_2;
    public GameObject L0_3;
    public GameObject L0_4;
    public GameObject L0_5;
    public GameObject L1_1;
    public GameObject L1_2;
    public GameObject L1_3;
    public GameObject L1_4;

    public GameObject black;

    static private int[] levelUnlocks = { 1, 0, 0, 0, 0, 1, 0, 0, 0 };

    static public void UnlockLevel(int level)
    {
        levelUnlocks[level] = 1;
    }

    void Start()
    {
        if (levelUnlocks[0] == 0)
        {
            L0_1.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[1] == 0)
        {
            L0_2.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[2] == 0)
        {
            L0_3.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[3] == 0)
        {
            L0_4.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[4] == 0)
        {
            L0_5.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[5] == 0)
        {
            L1_1.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[6] == 0)
        {
            L1_2.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[7] == 0)
        {
            L1_3.GetComponent<Text>().text = "Locked";
        }
        if (levelUnlocks[8] == 0)
        {
            L1_4.GetComponent<Text>().text = "Locked";
        }
        game = GameManager.instance;
        game.practiceMode = false;

        // Set starting enable/disable
        MainMenu.SetActive(true);
        HeistMenu.SetActive(false);
        PracticeMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
    }

    public void ToMain()
    {
        StartCoroutine(ToMainCoroutine());
    }

    public IEnumerator ToMainCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        MainMenu.SetActive(true);
        HeistMenu.SetActive(false);
        PracticeMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
        yield return StartCoroutine(FadeIn());
    }

    public void ToHeistMenu()
    {
        StartCoroutine(ToHeistMenuCoroutine());
    }

    public IEnumerator ToHeistMenuCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        MainMenu.SetActive(false);
        HeistMenu.SetActive(true);
        PracticeMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
        yield return StartCoroutine(FadeIn());
    }

    public void ToPracticeMenu()
    {
        StartCoroutine(ToPracticeMenuCoroutine());
    }

    public IEnumerator ToPracticeMenuCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        PracticeMenu.SetActive(true);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
        yield return StartCoroutine(FadeIn());
    }

    public void ToHeistTutorial()
    {
        StartCoroutine(ToHeistTutorialCoroutine());
    }

    public IEnumerator ToHeistTutorialCoroutine()
    {
        yield return StartCoroutine(FadeOut());
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        PracticeMenu.SetActive(false);
        HeistTutorial.SetActive(true);
        Heist1.SetActive(false);
        yield return StartCoroutine(FadeIn());
    }

    public void ToHeist1()
    {
        StartCoroutine(ToHeist1Coroutine());
    }

    public IEnumerator ToHeist1Coroutine()
    {
        yield return StartCoroutine(FadeOut());
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        PracticeMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(true);
        yield return StartCoroutine(FadeIn());
    }

    public void QuitButton() => Application.Quit();

    public void Heist0StartButton() => game.EnterPlayScene("0-0-intro");

    public void Heist1StartButton() => game.EnterPlanScene("1.1 (Outside)");

    public void PracticeLevelStartButton(int levelNum)
    {
        Debug.Log("button click");
        if (levelUnlocks[levelNum] == 1)
        {
            game.practiceMode = true;
            if (levelNum == 0)
            {
                game.EnterPlanScene("0.1 (Basic)");
            }
            else if (levelNum == 1)
            {
                game.EnterPlanScene("0.2 (Crate)");
            }
            else if (levelNum == 2)
            {
                game.EnterPlanScene("0.3 (Freeze)");
            }
            else if (levelNum == 3)
            {
                game.EnterPlanScene("0.4 (Distraction)");
            }
            else if (levelNum == 4)
            {
                game.EnterPlanScene("0.4 (Distraction)"); //Replace with actual scene names
            }
            else if (levelNum == 5)
            {
                game.EnterPlanScene("1.1 (Outside)");
            }
            else if (levelNum == 6)
            {
                game.EnterPlanScene("1.2 (Inside)");
            }
            else if (levelNum == 7)
            {
                game.EnterPlanScene("1.3 (Office)");
            }
            else if (levelNum == 8)
            {
                game.EnterPlanScene("1.4 (Escape)");
            }
        }
    }

    public IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float time_init = Time.time;
        float progress = (Time.time - time_init) / duration;
        // Start alpha at 0 then enable
        Color temp = black.GetComponent<Image>().color;
        temp.a = 0.0f;
        black.GetComponent<Image>().color = temp;
        black.SetActive(true);

        float a_init = 0.0f;
        float a_dest = 1.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - time_init) / duration;
            float a_new = Mathf.Lerp(a_init, a_dest, progress);
            float interval = a_new - black.GetComponent<Image>().color.a;

            temp.a += interval;
            black.GetComponent<Image>().color = temp;

            yield return null;
        }
        temp.a = a_dest;
        black.GetComponent<Image>().color = temp;
    }

    public IEnumerator FadeIn()
    {
        float duration = 0.5f;
        float time_init = Time.time;
        float progress = (Time.time - time_init) / duration;
        // Start alpha at 1
        Color temp = black.GetComponent<Image>().color;
        temp.a = 1.0f;
        black.GetComponent<Image>().color = temp;

        float a_init = 1.0f;
        float a_dest = 0.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - time_init) / duration;
            float a_new = Mathf.Lerp(a_init, a_dest, progress);
            float interval = a_new - black.GetComponent<Image>().color.a;

            temp.a += interval;
            black.GetComponent<Image>().color = temp;

            yield return null;
        }
        // Disable image
        temp.a = a_dest;
        black.GetComponent<Image>().color = temp;

        black.SetActive(false);
    }
}
