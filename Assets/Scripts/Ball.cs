using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField]
    float ballVelocity = 7f;

    [SerializeField]
    float startBallDelay = 1f;

    [SerializeField]
    AudioSource audioSource;

    Rigidbody myRigidbody;
    GameManager gameManager;
    Helix helix;
    bool gameStopRequired = false;
    bool destroyPlatformRequired = false;

    private void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        gameManager = FindObjectOfType<GameManager>();
        helix = FindObjectOfType<Helix>();

        gameManager.gameContinueOrder += StartBall;
        gameManager.gameStopOrder += StopBall;
        gameManager.destroyPlatformOrder += DestroyPlatformOrderReceived;
    }

    private void Update() {
        if (helix.HelixBuildFinished && !gameStopRequired) {
            Invoke("StartBall", startBallDelay);
        }
    }

    private void StartBall() {
        myRigidbody.isKinematic = false;
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<BottomPlatform>()) {
            gameManager.OnBottomPlatformHit();
            return;
        }
        if (destroyPlatformRequired) {
            helix.DestroyActualPlatform();
            myRigidbody.velocity = new Vector3(0f, 5f, 0f);
            destroyPlatformRequired = false;
            return;
        }
        if (other.gameObject.GetComponent<TiltPart>()) {
            StopBall();
            helix.StopRotationControl();
            gameManager.OnTiltPartHit();
            return;
        }
        if (other.gameObject.GetComponent<PlatformOfBouncing>() && gameObject.transform.position.y > other.transform.position.y) {
            myRigidbody.velocity = new Vector3(0f, ballVelocity, 0f);
            audioSource.Play();
            gameManager.OnBouncingPartHit();
        }
    }

    private void StopBall() {
        myRigidbody.isKinematic = true;
        myRigidbody.velocity = Vector3.zero;
        gameStopRequired = true;
    }

    private void DestroyPlatformOrderReceived() {
        destroyPlatformRequired = true;
    }

}
