using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField]
    float ballVelocity = 7f;

    [SerializeField]
    AudioSource audioSource;

    Rigidbody myRigidbody;
    GameManager gameManager;
    HelixBuild helixBuild;
    bool gameStopRequired = false;

    private void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        gameManager = FindObjectOfType<GameManager>();
        helixBuild = FindObjectOfType<HelixBuild>();
    }

    private void Update() {
        if (helixBuild.HelixBuildFinished && !gameStopRequired) {
            myRigidbody.isKinematic = false;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<PlatformOfBouncing>() && gameObject.transform.position.y > other.transform.position.y) {
            myRigidbody.velocity = new Vector3(0f, ballVelocity, 0f);
            audioSource.Play();
        } else if (other.gameObject.GetComponent<TiltPart>()) {
            gameManager.OnTiltPartHit();
        } else if (other.gameObject.GetComponent<BottomPlatform>()) {
            gameManager.OnBottomPlatformHit();
        }
    }

    public void StopBall() {
        myRigidbody.isKinematic = true;
        gameStopRequired = true;
    }

}
