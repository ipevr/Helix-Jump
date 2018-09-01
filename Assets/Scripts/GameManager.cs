﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    GameObject actualPlatform = null;
    ScreenPanelController screenPanelController;
    LevelManager levelManager;
    int platformPassedCounter = 0;

    public GameObject ActualPlatform => actualPlatform;

    public delegate void GameContinueOrder();
    public event GameContinueOrder gameContinueOrder;
    public delegate void GameStopOrder();
    public event GameStopOrder gameStopOrder;


    // Use this for initialization
    void Start () {
        screenPanelController = FindObjectOfType<ScreenPanelController>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void OnLevelProgress(float progressInPercent) {
        screenPanelController.ShowLevelProgress(progressInPercent);
    }

    public void PlatformPassedCounter() {
        platformPassedCounter++;
        Debug.Log("Passed " + platformPassedCounter);
    }

    public void OnTiltPartHit() {
        AskPlayerAnotherTry();
    }

    public void OnBouncingPartHit() {
        platformPassedCounter = 0;
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
        if (gameContinueOrder != null) {
            gameContinueOrder();
        }
    }

    public void StopGame() {
        if (gameStopOrder != null) {
            gameStopOrder();
        }
    }

    void AskPlayerAnotherTry() {
        screenPanelController.ShowPlayAgainPanel();
        StopGame();
    }


}
