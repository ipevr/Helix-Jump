using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOfBouncing : MonoBehaviour {

    public delegate void OnBallHitPlatformOfBouncing();
    public event OnBallHitPlatformOfBouncing onBallHitPlatformOfBouncing;

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<Ball>()) {
            if (onBallHitPlatformOfBouncing != null) { // other classes must have subscribed to the event
                onBallHitPlatformOfBouncing();
            }
        }
    }

}
