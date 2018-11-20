using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour {

    [SerializeField]
    int numberOfPlatforms = 8;

    [SerializeField]
    float rotationDamping = 0.8f;
    
    PlatformExitObserver[] platformExitObservers;
    CameraController cameraController;
    GameManager gameManager;
    HelixBuild helixBuild;
    float angle = 0f;
    int platformNumber = 0;
    bool rotationStopped = true;
    bool helixBuildFinished = false;

    bool rotationPaused = false;

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

        gameManager.gameContinueOrder += StartRotationControl;
        gameManager.gameStopOrder += StopRotationControl;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump")) {
            rotationPaused = !rotationPaused;
            Debug.Log("paused" + rotationPaused);
        }
        // rotate Helix with mouse movement
        if (helixBuild.HelixBuildFinished && !helixBuildFinished) {
            rotationStopped = false;
            helixBuildFinished = true;
        }
        if (!rotationStopped) {
            if (Input.touchCount == 1) {
                Touch myTouch = Input.GetTouch(0);
                angle += myTouch.deltaPosition.x * rotationDamping;
#if UNITY_EDITOR
            } else {
                if (!rotationPaused) {
                    angle = Input.mousePosition.x;
                }
#endif
            }
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
        }
    }

    //private void OnGUI() {
    //    foreach (Touch touch in Input.touches) {
    //        string message = "";
    //        message += "ID: " + touch.fingerId + "\n";
    //        message += "Phase: " + touch.phase.ToString() + "\n";
    //        message += "TapCount: " + touch.tapCount + "\n";
    //        message += "Pos X: " + touch.position.x + "\n";
    //        message += "Pos Y: " + touch.position.y + "\n";

    //        int num = touch.fingerId;
    //        GUI.Label(new Rect(0 + 130 * num, 0, 120, 100), message);


    //    }
    //}

    void OnBallEnteredPlatformExitObserver() {
        MoveCamera();
        float progressInPercent = 100f / platformExitObservers.Length;
        platformNumber++;
        gameManager.OnLevelProgress(progressInPercent);
        gameManager.PlatformPassedCounter();
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

    public void StartRotationControl() {
        rotationStopped = false;
    }

}
