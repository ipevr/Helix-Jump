using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    float switchToNextLevelWaitTime = 2f;

    const int optionsScreenIndex = 0;
    const int startLevelIndex = 1;

    int numberOfLevels = 0;
    int actualLevelIndex = 1;
    int lastGameLevelIndex = 1;
    ScreenPanelController screenPanelController;

    public int NumberOfLevelsInGame => SceneManager.sceneCountInBuildSettings - 1;

    public bool IsLastSceneIndex => (SceneManager.GetActiveScene().buildIndex == NumberOfLevelsInGame);

    public int ActulSceneIndex => SceneManager.GetActiveScene().buildIndex;

    // TODO: A build will start with buildindex 0, which is the options screen --> that's not good!

    private void Start() {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        actualLevelIndex = PlayerPrefsManager.GetActualLevel();
        if (!PlayerPrefsManager.ActualLevelKeyExists) {
            PlayerPrefsManager.SetActualLevel(lastGameLevelIndex);
        }
        if ((ActulSceneIndex != actualLevelIndex) && (ActulSceneIndex != optionsScreenIndex)) {
            SceneManager.LoadScene(actualLevelIndex);
        }

    }

    public void NextLevel() {
        screenPanelController.ShowNextLevelPanel(ActulSceneIndex + 1);
        StartCoroutine(LoadLevelAfterTime(ActulSceneIndex + 1, switchToNextLevelWaitTime));
        PlayerPrefsManager.SetActualLevel(ActulSceneIndex + 1);
    }

    public void ResetLevelstorage() {
        StartFromBeginning();
    }

    public void CallOptionsScreen() {
        lastGameLevelIndex = ActulSceneIndex;
        SceneManager.LoadScene(optionsScreenIndex);
    }

    public void BackToGame() {
        SceneManager.LoadScene(lastGameLevelIndex);
    }

    public void StartFromBeginning() {
        PlayerPrefsManager.SetActualLevel(startLevelIndex);
        SceneManager.LoadScene(startLevelIndex);
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
