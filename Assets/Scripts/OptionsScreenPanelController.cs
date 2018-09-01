using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScreenPanelController : MonoBehaviour {

    [SerializeField]
    GameObject areYouSurePanel;

    LevelManager levelManager;

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        areYouSurePanel.SetActive(false);
    }

    public void ShowAreYouSurePanel() {
        areYouSurePanel.SetActive(true);
    }

    public void HideAreYouSurePanel() {
        areYouSurePanel.SetActive(false);
    }

    public void BackToGame() {
        levelManager.BackToGame();
    }

}
