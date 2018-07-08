using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : MonoBehaviour {

    [SerializeField]
    float movingTime = 2f;
    [SerializeField]
    float movingDelayTime = 0.1f;
    
    PlatformExitObserver[] platformExitObservers;
    CameraController cameraController;
    GameManager gameManager;
    float moveValue = -4f;
    bool rotationStopped = false;
 
	// Use this for initialization
	void Start () {
        platformExitObservers = GetComponentsInChildren<PlatformExitObserver>();
        cameraController = FindObjectOfType<CameraController>();
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < platformExitObservers.Length; i++) {
            platformExitObservers[i].onBallEnteredPlatformExitObserver += OnBallEnteredPlatformExitObserver;
        }
	}
	
	// Update is called once per frame
	void Update () {
        // rotate Helix with mouse movement
        if (!rotationStopped) {
            float angle = Input.mousePosition.x;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
        }
    }

    void OnBallEnteredPlatformExitObserver() {
        Invoke("MoveCamera", movingDelayTime);
        float progressInPercent = 100f / platformExitObservers.Length;
        gameManager.OnLevelProgress(progressInPercent);
    }

    void MoveCamera() {
        cameraController.MoveCamera(moveValue, movingTime);
    }

    public void StopRotationControl() {
        rotationStopped = true;
    }

}
