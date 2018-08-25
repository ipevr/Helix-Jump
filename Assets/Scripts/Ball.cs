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
    HelixBuild helixBuild;
    bool gameStopRequired = false;

    private void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        gameManager = FindObjectOfType<GameManager>();
        helixBuild = FindObjectOfType<HelixBuild>();
        helix = FindObjectOfType<Helix>();
    }

    private void Update() {
        if (helixBuild.HelixBuildFinished && !gameStopRequired) {
            Invoke("StartBall", startBallDelay);
        }
    }

    private void StartBall() {
        myRigidbody.isKinematic = false;
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<PlatformOfBouncing>() && gameObject.transform.position.y > other.transform.position.y) {
            myRigidbody.velocity = new Vector3(0f, ballVelocity, 0f);
            audioSource.Play();
        } else if (other.gameObject.GetComponent<TiltPart>()) {
            StopBall();
            helix.StopRotationControl();
            gameManager.OnTiltPartHit();
        } else if (other.gameObject.GetComponent<BottomPlatform>()) {
            gameManager.OnBottomPlatformHit();
        }
    }

    public void StopBall() {
        myRigidbody.isKinematic = true;
        myRigidbody.velocity = Vector3.zero;
        Debug.Log("stop");
        gameStopRequired = true;
    }

}
