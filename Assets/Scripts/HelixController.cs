using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour {

    [SerializeField]
    float movingTime = 2f;
    [SerializeField]
    float movingDelayTime = 0.1f;


    PlatformExitObserver[] platformExitObservers;
    Vector3 actualPosition;
    Vector3 newPosition;
 
	// Use this for initialization
	void Start () {
        platformExitObservers = GetComponentsInChildren<PlatformExitObserver>();
        for (int i = 0; i < platformExitObservers.Length; i++) {
            platformExitObservers[i].onBallEnteredPlatformExitObserver += OnBallEnteredPlatformExitObserver;
        }
        actualPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Input.mousePosition.x;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }

    void OnBallEnteredPlatformExitObserver() {
        newPosition = actualPosition + new Vector3(0f, 4f, 0f);
        actualPosition = newPosition;
        Invoke("MovePlatform", movingDelayTime);
    }

    void MovePlatform() {
        StartCoroutine(MoveOverSeconds(gameObject, newPosition, movingTime));

    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endPosition, float movingTime) {
        float elaspedTime = 0f;
        Vector3 startingPosition = objectToMove.transform.position;
        while (elaspedTime < movingTime) {
            objectToMove.transform.position = Vector3.Lerp(startingPosition, endPosition, (elaspedTime / movingTime));
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPosition;
    }
}
