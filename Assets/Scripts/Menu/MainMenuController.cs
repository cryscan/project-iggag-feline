using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public GameObject MainMenu;
    // Start is called before the first frame update
    void Start() {
        MainMenu.SetActive(true);
    }

    public void QuitButton() {
    	Application.Quit();
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
}
