using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 toPosition;
    float moveTime;
    bool moveObjectRequired = false;

    private void LateUpdate() {
        if (moveObjectRequired) {
            StartCoroutine(MoveOverSeconds(gameObject, toPosition, moveTime));
            moveObjectRequired = false;
        }
    }

    public void MoveCamera(float moveValue, float time) {
        Vector3 actualPosition = gameObject.transform.position;
        toPosition = new Vector3(actualPosition.x, actualPosition.y + moveValue, actualPosition.z);
        moveTime = time;
        moveObjectRequired = true;
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
