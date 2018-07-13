using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 toPosition;
    Ball ball;
    float moveTime;
    bool moveObjectRequired = false;

    void Start() {
        ball = FindObjectOfType<Ball>();
    }

    void LateUpdate() {
        if (moveObjectRequired) {
            StartCoroutine(MoveWithBall(gameObject, toPosition));
            moveObjectRequired = false;
        }
    }

    public void MoveCamera(float moveValue, float time) {
        Vector3 actualPosition = gameObject.transform.position;
        toPosition = new Vector3(actualPosition.x, actualPosition.y + moveValue, actualPosition.z);
        moveTime = time;
        moveObjectRequired = true;
    }

    IEnumerator MoveWithBall(GameObject objectToMove, Vector3 endPosition) {
        //float elaspedTime = 0f;
        Vector3 startingPosition = objectToMove.transform.position;
        while (objectToMove.transform.position.y > endPosition.y) {
            Debug.Log("objectToMove.transform.position.y" + objectToMove.transform.position.y);
            Debug.Log("endPosition.y" + endPosition.y);
            float delta = objectToMove.transform.position.y - ball.transform.position.y;
            Debug.Log("delta" + delta);
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, ball.transform.position.y - delta, objectToMove.transform.position.z);
            //elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPosition;
    }

}
