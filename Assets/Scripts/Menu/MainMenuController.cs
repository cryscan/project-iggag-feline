using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public GameObject MainMenu;
    public GameObject HeistMenu;
    // public GameObject Controls;
    public GameObject HeistTutorial;
    public GameObject Heist1;
    // Start is called before the first frame update
    void Start() {
        ToMain();
    }

    public void QuitButton() {
    	Application.Quit();
    }

    public void ToMain() {
        MainMenu.SetActive(true);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
    }

    public void ToHeistMenu() {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(true);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(false);
    }

    public void ToHeistTutorial() {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(true);
        Heist1.SetActive(false);
    }

    public void ToHeist1() {
        MainMenu.SetActive(false);
        HeistMenu.SetActive(false);
        HeistTutorial.SetActive(false);
        Heist1.SetActive(true);
    }

    public void TutorialBasicButton() {
    	UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial_Basic");
    }

    public void TutorialFreezeButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial_Freeze");
    }

    public void TutorialCrateButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial_Crate");
    }

    public void TutorialDistractionButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial_Distraction");
    }

    public void AdvancedLevelButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LAB_richard");
    }
}
