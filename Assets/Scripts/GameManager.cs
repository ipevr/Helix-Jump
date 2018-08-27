using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject actualPlatform = null;

    ScreenPanelController screenPanelController;
    LevelManager levelManager;
    Ball ball;
    Helix helix;
    int actualLevelIndex = 0;

    // Use this for initialization
    void Start () {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        levelManager = FindObjectOfType<LevelManager>();
        ball = FindObjectOfType<Ball>();
        helix = FindObjectOfType<Helix>();
    }

    public void OnLevelProgress(float progressInPercent) {
        screenPanelController.ShowLevelProgress(progressInPercent);
    }

    public void OnTiltPartHit() {
        AskPlayerAnotherTry();
    }

    public void OnBottomPlatformHit() {
        if (!levelManager.IsLastSceneIndex) {
            StopGame();
            levelManager.NextLevel();
        } else {
            GameWon();
        }
    }

    public void ExitGame() {
        Debug.Log("Quit application required.");
        Application.Quit();
    }

    public void PlayLevelAgain() {
        levelManager.PlayLevelAgain();
    }

    public void PlayGameAgain() {
        levelManager.StartFromBeginning();
    }

    public void GameWon() {
        StopGame();
        screenPanelController.ShowWonPanel();
    }

    public void SetActualPlatform(GameObject platformObject) {
        actualPlatform = platformObject;
    }

    public void ContinueGame() {
        ball.StartBall();
        helix.StartRotationControl();
    }

    public void StopGame() {
        ball.StopBall();
        helix.StopRotationControl();
    }

    void AskPlayerAnotherTry() {
        screenPanelController.ShowPlayAgainPanel();
        StopGame();
    }


}
