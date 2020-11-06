using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    GameManager game;

    void Start()
    {
        game = GameManager.instance;
    }

    public void QuitButton() => Application.Quit();

    public void TutorialBasicButton() => game.EnterPlanScene("Scenes/Tutorial_Basic");


    public void TutorialFreezeButton()
    {
        game.EnterPlanScene("Tutorial_Freeze");
    }

    public void TutorialCrateButton()
    {
        game.EnterPlanScene("Tutorial_Crate");
    }

    public void TutorialDistractionButton()
    {
        game.EnterPlanScene("Tutorial_Distraction");
    }

    public void AdvancedLevelButton()
    {
        game.EnterPlanScene("Lab_richard");
    }
}
