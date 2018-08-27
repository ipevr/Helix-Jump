using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    GameObject areYouSurePanel;

    string textBubbleActualLevel = "";
    string textBubbleNextLevel = "";
    int numberOfLevels = 0;

    void Start() {
        playAgainPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        nextLevelPanel.SetActive(false);
        areYouSurePanel.SetActive(false);
        int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        numberOfLevels = SceneManager.sceneCountInBuildSettings;
        textBubbleActualLevel = levelIndex.ToString();
        if (levelIndex < numberOfLevels) {
            textBubbleNextLevel = (levelIndex + 1).ToString();
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

    public void ShowNextLevelPanel(int level) {
        nextLevelPanel.SetActive(true);
        string nextLevelText = "Level " + level.ToString();
        nextLevelPanel.GetComponentInChildren<Text>().text = nextLevelText;
    }

    public void ShowAreYouSurePanel() {
        areYouSurePanel.SetActive(true);
    }

}
