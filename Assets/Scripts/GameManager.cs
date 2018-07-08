﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    ScreenPanelController screenPanelController;
    Ball ball;
    Helix helix;
    int numberOfLevels = 0;

    // Use this for initialization
    void Start () {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        ball = FindObjectOfType<Ball>();
        helix = FindObjectOfType<Helix>();
        numberOfLevels = SceneManager.sceneCountInBuildSettings;
    }

    public void OnLevelProgress(float progressInPercent) {
        screenPanelController.ShowLevelProgress(progressInPercent);
    }

    public void OnTiltPartHit() {
        Debug.Log("Tilt Part was touched");
        AskPlayerAnotherTry();
    }

    public void OnBottomPlatformHit() {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < numberOfLevels - 1) {
            SceneManager.LoadScene(sceneIndex + 1);
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

    void AskPlayerAnotherTry() {
        screenPanelController.ShowPlayAgainPanel();
        StopGame();
    }

    void StopGame() {
        ball.StopBall();
        helix.StopRotationControl();
    }

}
