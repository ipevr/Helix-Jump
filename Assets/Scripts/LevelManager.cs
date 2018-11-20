using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    float switchToNextLevelWaitTime = 2f;

    const int startLevelIndex = 0;

    int actualLevelIndex = 0;
    int lastGameLevelIndex = 0;

    public int NumberOfLevelsInGame => SceneManager.sceneCountInBuildSettings - 1;
    public bool IsLastSceneIndex => (SceneManager.GetActiveScene().buildIndex == NumberOfLevelsInGame - 1);
    public int ActulSceneIndex => SceneManager.GetActiveScene().buildIndex;
    int OptionsScreenIndex => SceneManager.sceneCountInBuildSettings - 1;

    private void Start() {
        if (!PlayerPrefsManager.ActualLevelKeyExists) {
            PlayerPrefsManager.SetActualLevel(lastGameLevelIndex);
        }
        actualLevelIndex = PlayerPrefsManager.GetActualLevel();
        if ((ActulSceneIndex != actualLevelIndex) && (ActulSceneIndex != OptionsScreenIndex)) {
            SceneManager.LoadScene(actualLevelIndex);
        }
    }

    public void NextLevel() {
        StartCoroutine(LoadLevelAfterTime(ActulSceneIndex + 1, switchToNextLevelWaitTime));
        PlayerPrefsManager.SetActualLevel(ActulSceneIndex + 1);
    }

    public void ResetLevelstorage() {
        StartFromBeginning();
    }

    public void CallOptionsScreen() {
        lastGameLevelIndex = ActulSceneIndex;
        SceneManager.LoadScene(OptionsScreenIndex);
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
