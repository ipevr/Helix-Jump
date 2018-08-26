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
    float angleOfTiltPart = 30f;

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
    }

    void CreateNormalBouncingParts() {
        float rotation = positionOfGap + 90f;
        for (int i = 0; i < 3; i++) {
            GameObject bouncingPart = Instantiate(normalBounncingPartPrefab, platform.transform);
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
        if (restAngle <= 0) {
            Debug.LogError("The rest angle for the tilt parts is too low. Reduce the number of tilt parts or their angle.");
        }
        // create 'number' numbers between 0 and 1
        float sum = 0f;
        for (int i = 0; i < number; i++) {
            alphas[i] = UnityEngine.Random.Range(0f, 1f);
            sum += alphas[i];
        }
        for (int i = 0; i < number; i++) {
            // divide each n by its sum and multiply it with restAngle
            alphas[i] = (alphas[i] / sum) * restAngle;
            if (i == 0) {
                // first angle must be > gapAngle
                alphas[i] += gapAngle + positionOfGap;
            } else {
                alphas[i] += angleOfTiltPart;
            }
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
                rotatingParts[i].transform.rotation = Quaternion.Euler(rotatingParts[i].transform.rotation.x, 
                                                                       startPosition[i].eulerAngles.y + rot,
                                                                       rotatingParts[i].transform.rotation.z);
            }
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // secure that the angle is set correct after rotatingTime
        for (int i = 0; i < numberOfParts; i++) {
            rotatingParts[i].transform.rotation = Quaternion.Euler(rotatingParts[i].transform.rotation.x,
                                                                   startPosition[i].eulerAngles.y + rotations[0],
                                                                   rotatingParts[i].transform.rotation.z);
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
