using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField]
    float ballVelocity = 7f;

    [SerializeField]
    AudioSource audioSource;

    Rigidbody myRigidbody;

    private void Start() {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<PlatformOfBouncing>() && gameObject.transform.position.y > other.transform.position.y) {
            myRigidbody.velocity = new Vector3(0f, ballVelocity, 0f);
            audioSource.Play();
        }
    }

}
