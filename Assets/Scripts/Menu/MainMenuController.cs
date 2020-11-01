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

    public void LevelButton() {
    	UnityEngine.SceneManagement.SceneManager.LoadScene("LAB_jszion");
    }
}
