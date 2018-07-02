using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    LevelProgressController levelProgressController;

    // Use this for initialization
    void Start () {
        levelProgressController = FindObjectOfType<LevelProgressController>();
    }

    public void OnLevelProgress(float progressInPercent) {
        levelProgressController.ShowLevelProgress(progressInPercent);
    }
}
