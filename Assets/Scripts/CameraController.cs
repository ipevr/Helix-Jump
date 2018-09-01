using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    float movingTime = 0.2f;

    Vector3 startingPosition;
    Vector3 actualPosition;
    Vector3 toPosition;
    float moveTime;
    bool moveObjectRequired = false;
    Ball ball;
    float deltaY;
    float platformPositionY;
    PlatformOfBouncing[] platformOfBouncings;
    GameObject targetPlatform;
    GameManager gameManager;

    private void Start() {
        startingPosition = transform.position;
        ball = FindObjectOfType<Ball>();
        gameManager = FindObjectOfType<GameManager>();
        platformOfBouncings = FindObjectsOfType<PlatformOfBouncing>();
        for (int i = 0; i < platformOfBouncings.Length; i++) {
            platformOfBouncings[i].onBallHitPlatformOfBouncing += StopCamera;
        }
    }

    private void LateUpdate() {
        if (moveObjectRequired) {
            // Camera must move together with ball until the ball hits a platform
            float ballPositionY = ball.gameObject.transform.position.y;
            toPosition = new Vector3(actualPosition.x, ballPositionY + deltaY, actualPosition.z);
            transform.position = toPosition;
        }
    }

    public void MoveCamera() {
        actualPosition = gameObject.transform.position;
        float ballPositionY = ball.gameObject.transform.position.y;
        deltaY = actualPosition.y - ballPositionY;
        moveObjectRequired = true;
    }

    void StopCamera() {
        if (moveObjectRequired) {
            // Move to the correct camera view of the actual platform
            Vector3 endPosition = gameManager.ActualPlatform.transform.position + startingPosition;
            StartCoroutine(MoveOverSeconds(gameObject, endPosition, movingTime));
        }
        moveObjectRequired = false;
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
