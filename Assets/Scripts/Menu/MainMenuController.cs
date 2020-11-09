using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    GameManager game;
    public GameObject MainMenu;
    public GameObject HeistMenu;
    // public GameObject Controls;
    public GameObject HeistTutorial;
    public GameObject Heist1;

    void Start()
    {
        game = GameManager.instance;
        ToMain();
    }

    public void ToMain()
    {
        MainMenu.SetActive(true);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
    }

    public void ToHeistMenu()
    {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(true);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
    }

    public void ToHeistTutorial()
    {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(true);
        Heist1.SetActive(false);
    }

    public void ToHeist1()
    {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(true);
    }

    public void QuitButton() => Application.Quit();

    public void Heist0StartButton() => game.EnterPlanScene("0.1 (Basic)");

    public void Heist1StartButton() => game.EnterPlanScene("1.1 (Outside)");
}
