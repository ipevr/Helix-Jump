using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformExitObserver : MonoBehaviour {

    public delegate void OnBallEnteredPlatformExitObserver();
    public event OnBallEnteredPlatformExitObserver onBallEnteredPlatformExitObserver;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<BallController>()) {
            onBallEnteredPlatformExitObserver();
            // Disable collider of passed platform to prevent ball exit the collider a second time
            gameObject.GetComponent<Collider>().enabled = false;
            Debug.Log(gameObject.transform.parent.gameObject + " disabled");
        }
    }
}
