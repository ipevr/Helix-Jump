using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField]
    float ballVelocity = 7f;


    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<PlatformOfBouncing>()) {
            GetComponent<Rigidbody>().velocity = new Vector3(0f, ballVelocity, 0f);
        }
    }
}
