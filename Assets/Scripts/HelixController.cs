using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour {

    [SerializeField]
    float movingTime = 2f;
    [SerializeField]
    float movingDelayTime = 0.1f;
    
    PlatformExitObserver[] platformExitObservers;
    CameraController cameraController;
    float moveValue = -4f;
 
	// Use this for initialization
	void Start () {
        platformExitObservers = GetComponentsInChildren<PlatformExitObserver>();
        cameraController = FindObjectOfType<CameraController>();
        for (int i = 0; i < platformExitObservers.Length; i++) {
            platformExitObservers[i].onBallEnteredPlatformExitObserver += OnBallEnteredPlatformExitObserver;
        }
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Input.mousePosition.x;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }

    void OnBallEnteredPlatformExitObserver() {
        Invoke("MoveCamera", movingDelayTime);
    }

    void MoveCamera() {
        cameraController.MoveCamera(moveValue, movingTime);
    }

}
