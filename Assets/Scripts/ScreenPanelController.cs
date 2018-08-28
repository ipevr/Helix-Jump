using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPanelController : MonoBehaviour {

    [SerializeField]
    LevelProgressController levelProgressController;

    [SerializeField]
    GameObject bubbleActualLevel;

    [SerializeField]
    GameObject bubbleNextLevel;

    [SerializeField]
    GameObject playAgainPanel;

    [SerializeField]
    GameObject gameWonPanel;

    [SerializeField]
    GameObject nextLevelPanel;

    LevelManager levelManager;
    string textBubbleActualLevel = "";
    string textBubbleNextLevel = "";

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        playAgainPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        textBubbleActualLevel = levelManager.ActulSceneIndex.ToString();
        if (levelManager.ActulSceneIndex < levelManager.NumberOfLevelsInGame) {
            textBubbleNextLevel = (levelManager.ActulSceneIndex + 1).ToString();
        } else {
            textBubbleNextLevel = "E";
        }
        bubbleActualLevel.GetComponentInChildren<Text>().text = textBubbleActualLevel;
        bubbleNextLevel.GetComponentInChildren<Text>().text = textBubbleNextLevel;
    }

    public void ShowLevelProgress(float progressInPercent) {
        levelProgressController.ShowLevelProgress(progressInPercent);
    }

    public void ShowPlayAgainPanel() {
        playAgainPanel.SetActive(true);
    }

    public void ShowWonPanel() {
        gameWonPanel.SetActive(true);
    }

    public void ShowOptionsScreen() {
        levelManager.CallOptionsScreen();
    }

    public void ShowNextLevelPanel(int level) {
        nextLevelPanel.SetActive(true);
        string nextLevelText = "Level " + level.ToString();
        nextLevelPanel.GetComponentInChildren<Text>().text = nextLevelText;
    }

}
