using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    float switchToNextLevelWaitTime = 2f;

    int numberOfLevels = 0;
    int actualLevelIndex = 0;
    ScreenPanelController screenPanelController;

    public bool IsLastSceneIndex => (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1);

    public int ActulSceneIndex => SceneManager.GetActiveScene().buildIndex;

    private void Start() {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        actualLevelIndex = PlayerPrefsManager.GetActualLevel();
        if (ActulSceneIndex != actualLevelIndex) {
            SceneManager.LoadScene(actualLevelIndex);
        }

    }

    public void NextLevel() {
        screenPanelController.ShowNextLevelPanel(ActulSceneIndex + 2);
        StartCoroutine(LoadLevelAfterTime(ActulSceneIndex + 1, switchToNextLevelWaitTime));
        PlayerPrefsManager.SetActualLevel(ActulSceneIndex + 1);
    }

    public void ResetLevelstorage() {
        StartFromBeginning();
    }

    public void StartFromBeginning() {
        PlayerPrefsManager.SetActualLevel(0);
        SceneManager.LoadScene(0);
    }

    public void PlayLevelAgain() {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    IEnumerator LoadLevelAfterTime(int levelIndex, float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(levelIndex);
    }

}
