using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    bool velocitySaved = false;
    Vector3 savedVelocity;

    void OnCollisionEnter(Collision collision) {
        if (!velocitySaved) {
            savedVelocity = GetComponent<Rigidbody>().velocity;
            velocitySaved = true;
        }
        GetComponent<Rigidbody>().velocity = savedVelocity;
    }
}
