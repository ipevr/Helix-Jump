using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour {

    [SerializeField]
    int numberOfPlatforms = 8;
    
    PlatformExitObserver[] platformExitObservers;
    CameraController cameraController;
    GameManager gameManager;
    HelixBuild helixBuild;
    float moveValue = -4f;
    int platformNumber = 0;
    bool rotationStopped = true;

    private void Awake() {
        // Helix has to be built on Awake because other classes are expecting components of the Helix on Start
        helixBuild = GetComponent<HelixBuild>();
        if (helixBuild) {
            helixBuild.CreatePlatforms(numberOfPlatforms);
        }
    }

    // Use this for initialization
    void Start () {
        platformExitObservers = GetComponentsInChildren<PlatformExitObserver>();
        cameraController = FindObjectOfType<CameraController>();
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < platformExitObservers.Length; i++) {
            platformExitObservers[i].onBallEnteredPlatformExitObserver += OnBallEnteredPlatformExitObserver;
        }
        gameManager.SetActualPlatform(platformExitObservers[0].transform.parent.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        // rotate Helix with mouse movement
        if (helixBuild.HelixBuildFinished) {
            rotationStopped = false;
        }
        if (!rotationStopped) {
            float angle = Input.mousePosition.x;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
        }
    }

    void OnBallEnteredPlatformExitObserver() {
        MoveCamera();
        float progressInPercent = 100f / platformExitObservers.Length;
        platformNumber++;
        gameManager.OnLevelProgress(progressInPercent);
        if (platformNumber < platformExitObservers.Length) {
            gameManager.SetActualPlatform(platformExitObservers[platformNumber].transform.parent.gameObject);
        } else {
            gameManager.SetActualPlatform(helixBuild.GetBottomPlatform());
        }
    }

    void MoveCamera() {
        cameraController.MoveCamera();
    }

    public void StopRotationControl() {
        rotationStopped = true;
    }

}
