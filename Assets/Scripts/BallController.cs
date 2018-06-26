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
        Debug.Log("Collision with " + other.gameObject);
        if (other.gameObject.GetComponent<PlatformOfBouncing>()) {
            myRigidbody.velocity = new Vector3(0f, ballVelocity, 0f);
            audioSource.Play();
        }
    }

}
