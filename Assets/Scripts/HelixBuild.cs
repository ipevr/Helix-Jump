using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixBuild : MonoBehaviour {

    [SerializeField]
    GameObject helixGameObject;

    [SerializeField]
    GameObject normalBounncingPartPrefab;

    [SerializeField]
    GameObject tiltPartPrefab;
    [SerializeField]
    float angleOfTiltPart = 12f;

    [SerializeField]
    GameObject bottomPrefab;

    [SerializeField]
    GameObject platformExitObserverPrefab;

    [SerializeField]
    float buildTime = 0.6f;

    float positionOfGap = 0f;
    public float randomRotationOffset = 90f;
    public int numberOfTiltParts = 3;
    float yPosition = 0f;
    bool rotatingFinished = false;
    bool firstPlacement = true;
    public bool HelixBuildFinished { get { return rotatingFinished; } }
    GameObject platform;
    GameObject bottom;

	// Use this for initialization
	void Start () {
		
	}

    public void CreatePlatforms(int number) {
        for (int i = 0; i < number; i++) {
            CreatePlatformGameObject();
            DefinePositionOfGap();
            CreateNormalBouncingParts();
            CreateTiltParts(numberOfTiltParts);
            CreatePlatformExitObserver();
        }
        CreateBottom();
    }

    public GameObject GetBottomPlatform() {
        return bottom.transform.parent.gameObject;
    }

    void CreatePlatformGameObject() {
        platform = new GameObject("Platform");
        platform.transform.parent = helixGameObject.transform;
        platform.transform.position = new Vector3(0f, yPosition, 0f);
        yPosition -= 4f;
    }

    void DefinePositionOfGap() {
        //positionOfGap = UnityEngine.Random.Range(0f, 360f);
        positionOfGap = 0f;
    }

    void CreateNormalBouncingParts() {
        float rotation = positionOfGap + 90f;
        for (int i = 0; i < 3; i++) {
            GameObject bouncingPart = Instantiate(normalBounncingPartPrefab, platform.transform);
            //bouncingPart.transform.Rotate(Vector3.up * rotation);
            bouncingPart.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
            rotation += 90f;
        }
    }

    void CreateTiltParts(int number) {
        GameObject[] myTiltParts = new GameObject[number];
        for (int i = 0; i < number; i++) {
            myTiltParts[i] = Instantiate(tiltPartPrefab, platform.transform);
        }
        float buildtimePerTiltPart = buildTime / number;

        float[] tiltPartAngles = CalculateTiltPartAngles(90f, number, angleOfTiltPart);
        firstPlacement = true;
        StartCoroutine(RotateOverSeconds(myTiltParts, tiltPartAngles, buildtimePerTiltPart));
    }

    private float[] CalculateTiltPartAngles(float gapAngle, int number, float angleOfTiltPart) {
        float[] alphas = new float[number];
        float restAngle = 360 - (gapAngle + (number * angleOfTiltPart));
        //Debug.Log("restangle " + restAngle);
        for (int i = 0; i < number; i++) {
            float randomAngle = UnityEngine.Random.Range(0f, restAngle);
            restAngle -= randomAngle;
            alphas[i] = randomAngle;
            //Debug.Log("alpha " + i + " = " + alphas[i]);
            //Debug.Log("restangle " + restAngle);
        }
        return alphas;
    }

    void CreatePlatformExitObserver() {
        GameObject myPlatformExitObserver = Instantiate(platformExitObserverPrefab, platform.transform);
    }

    void CreateBottom() {
        CreatePlatformGameObject();
        bottom = Instantiate(bottomPrefab, platform.transform);
    }

    IEnumerator RotateOverSeconds(GameObject[] rotatingParts, float[] rotations, float rotatingTime) {
        float elaspedTime = 0f;
        int numberOfParts = rotatingParts.Length;
        // store the start positions of each rotatingPart
        Quaternion[] startPosition = new Quaternion[numberOfParts];
        for (int i = 0; i < numberOfParts; i++) {
            if (firstPlacement) {
                rotations[i] += positionOfGap + 90f;
            }
            startPosition[i] = rotatingParts[i].transform.rotation;
        }
        firstPlacement = false;
        // rotate all parts over time
        while (elaspedTime < rotatingTime) {
            for (int i = 0; i < numberOfParts; i++) {
                //float rot = Mathf.Lerp(startPosition[i].eulerAngles.y, startPosition[i].eulerAngles.y + rotations[i], (elaspedTime / rotatingTime));
                float rot = Mathf.Lerp(0f, rotations[i], (elaspedTime / rotatingTime));
                float xAngle = rotatingParts[i].transform.rotation.x;
                float yAngle = startPosition[i].eulerAngles.y + rot;
                float zAngle = rotatingParts[i].transform.rotation.z;
                rotatingParts[i].transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
                rot = 0f;
            }
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("check");
        for (int i = 0; i < numberOfParts; i++) {
            rotatingParts[i].transform.rotation = Quaternion.Euler(rotatingParts[i].transform.rotation.x, startPosition[i].eulerAngles.y + rotations[i], rotatingParts[i].transform.rotation.z);
        }
        // reduce parts by 1 and call the function again for the rest of the parts until there are no parts left
        if (numberOfParts > 1) {
            //rotation = Random.Range(5f, 85f);
            //Debug.Log("rotation " + rotation);
            GameObject[] restOfRotatingParts = new GameObject[numberOfParts - 1];
            float[] restOfRotations = new float[numberOfParts - 1];
            for (int i = 0; i < numberOfParts - 1; i++) {
                restOfRotatingParts[i] = rotatingParts[i];
                restOfRotations[i] = rotations[i];
            }
            StartCoroutine(RotateOverSeconds(restOfRotatingParts, restOfRotations, rotatingTime));
        } else {
            rotatingFinished = true;
        }

    }
}
