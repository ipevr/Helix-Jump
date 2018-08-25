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
    public int numberOfTiltParts = 3;
    float yPosition = 0f;
    bool rotatingFinished = false;
    public bool HelixBuildFinished { get { return rotatingFinished; } }
    GameObject platform;
    GameObject bottom;

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
        positionOfGap = UnityEngine.Random.Range(0f, 360f);
        //positionOfGap = 0f;
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
        StartCoroutine(RotateOverSeconds(myTiltParts, tiltPartAngles, buildtimePerTiltPart));
    }

    private float[] CalculateTiltPartAngles(float gapAngle, int number, float angleOfTiltPart) {
        float[] alphas = new float[number];
        float restAngle = 360 - (gapAngle + (number * angleOfTiltPart)); // = 198 for 6 parts
        for (int i = 0; i < number; i++) {
            // TODO: the distribution of the parts must be optimized: at the moment the are large angles at the beginning and always very small angles in the end
            // the problem is increasing with the number of parts
            float randomAngle = UnityEngine.Random.Range(0f, restAngle);
            if (i == 0) {
                // first angle must be > gapAngle
                alphas[i] = randomAngle + gapAngle + positionOfGap;
            } else {
                // rest of angles must be larger than the angleOfTiltPart to avoid overlapping parts
                // the next angle must be calculated relative to the angle before
                alphas[i] = randomAngle + angleOfTiltPart;
            }
            restAngle -= randomAngle;
            Debug.Log("alpha " + i + " = " + alphas[i]);
        }
        //alphas[0] += positionOfGap + gapAngle;
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
            startPosition[i] = rotatingParts[i].transform.rotation;
        }
        // rotate all parts over time
        while (elaspedTime < rotatingTime) {
            for (int i = 0; i < numberOfParts; i++) {
                float rot = Mathf.Lerp(0f, rotations[0], (elaspedTime / rotatingTime));
                float xAngle = rotatingParts[i].transform.rotation.x;
                float yAngle = startPosition[i].eulerAngles.y + rot;
                float zAngle = rotatingParts[i].transform.rotation.z;
                rotatingParts[i].transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
            }
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // secure that the angle is set correct after rotatingTime
        for (int i = 0; i < numberOfParts; i++) {
            float xAngle = rotatingParts[i].transform.rotation.x;
            float yAngle = startPosition[i].eulerAngles.y + rotations[0];
            float zAngle = rotatingParts[i].transform.rotation.z;
            rotatingParts[i].transform.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
        }
        // reduce parts by 1 and call the function again for the rest of the parts until there are no parts left
        if (numberOfParts > 1) {
            //rotation = Random.Range(5f, 85f);
            //Debug.Log("rotation " + rotation);
            GameObject[] restOfRotatingParts = new GameObject[numberOfParts - 1];
            float[] restOfRotations = new float[numberOfParts - 1];
            for (int i = 1; i < numberOfParts; i++) {
                restOfRotations[i - 1] = rotations[i];
                restOfRotatingParts[i - 1] = rotatingParts[i];
            }
            StartCoroutine(RotateOverSeconds(restOfRotatingParts, restOfRotations, rotatingTime));
        } else {
            rotatingFinished = true;
        }

    }
}
