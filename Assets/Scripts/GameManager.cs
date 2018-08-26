using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    float switchToNextLevelWaitTime;

    public GameObject actualPlatform = null;

    ScreenPanelController screenPanelController;
    Ball ball;
    Helix helix;
    int numberOfLevels = 0;
    int actualLevelIndex = 0;

    // Use this for initialization
    void Start () {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        ball = FindObjectOfType<Ball>();
        helix = FindObjectOfType<Helix>();
        numberOfLevels = SceneManager.sceneCountInBuildSettings;
        actualLevelIndex = PlayerPrefsManager.GetActualLevel();
        Debug.Log("levelIndex " + actualLevelIndex);
        Debug.Log("SceneManager.sceneCount " + SceneManager.sceneCount);
        if (SceneManager.GetActiveScene().buildIndex != actualLevelIndex) {
            SceneManager.LoadScene(actualLevelIndex);
        }
    }

    public void OnLevelProgress(float progressInPercent) {
        screenPanelController.ShowLevelProgress(progressInPercent);
    }

    public void OnTiltPartHit() {
        AskPlayerAnotherTry();
    }

    public void OnBottomPlatformHit() {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < numberOfLevels - 1) {
            SwitchToLevel(sceneIndex + 2);
        } else {
            GameWon();
        }
    }

    public void ExitGame() {
        Debug.Log("Quit application required.");
        Application.Quit();
    }

    public void PlayLevelAgain() {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    public void PlayGameAgain() {
        int index = 0;
        SceneManager.LoadScene(index);
    }

    public void GameWon() {
        StopGame();
        screenPanelController.ShowWonPanel();
    }

    public void SetActualPlatform(GameObject platformObject) {
        actualPlatform = platformObject;
    }

    public void ResetLevelstorage() {
        PlayerPrefsManager.SetActualLevel(0);
        actualLevelIndex = 0;
        SceneManager.LoadScene(actualLevelIndex);
    }

    void AskPlayerAnotherTry() {
        screenPanelController.ShowPlayAgainPanel();
        StopGame();
    }

    void StopGame() {
        ball.StopBall();
        helix.StopRotationControl();
    }

    void SwitchToLevel(int level) {
        screenPanelController.ShowNextLevelPanel(level);
        StartCoroutine(LoadLevelAfterTime(level, switchToNextLevelWaitTime));
        PlayerPrefsManager.SetActualLevel(level - 1);
    }

    IEnumerator LoadLevelAfterTime(int level, float time) {
        StopGame();
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(level - 1);
    }

}
