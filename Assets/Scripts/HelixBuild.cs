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
    GameObject bottomPrefab;

    [SerializeField]
    GameObject platformExitObserverPrefab;

    [SerializeField]
    float buildTime = 0.6f;

    public float positionOfGap = 0f;
    public float randomRotationOffset = 90f;
    public int numberOfTiltParts = 3;
    float yPosition = 0f;
    bool rotatingFinished = false;
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
        //positionOfGap = Random.Range(0f, 360f);
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
        //float randomRotationOffset = Random.Range(95f, 175f);
        //float randomRotationOffset = 95f;
        float rotation = positionOfGap + randomRotationOffset;
        float buildtimePerTiltPart = buildTime / number;
        StartCoroutine(RotateOverSeconds(myTiltParts, rotation, buildtimePerTiltPart));
    }

    void CreatePlatformExitObserver() {
        GameObject myPlatformExitObserver = Instantiate(platformExitObserverPrefab, platform.transform);
    }

    void CreateBottom() {
        CreatePlatformGameObject();
        bottom = Instantiate(bottomPrefab, platform.transform);
    }

    IEnumerator RotateOverSeconds(GameObject[] rotatingParts, float rotation, float rotatingTime) {
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
                float rot = Mathf.Lerp(startPosition[i].eulerAngles.y, startPosition[i].eulerAngles.y + rotation, (elaspedTime / rotatingTime));
                rotatingParts[i].transform.rotation = Quaternion.Euler(rotatingParts[i].transform.rotation.x, rot, rotatingParts[i].transform.rotation.z);
            }
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < numberOfParts; i++) {
            rotatingParts[i].transform.rotation = Quaternion.Euler(rotatingParts[i].transform.rotation.x, startPosition[i].eulerAngles.y + rotation, rotatingParts[i].transform.rotation.z);
        }
        // reduce parts by 1 and call the function again for the rest of the parts until there are no parts left
        if (numberOfParts > 1) {
            //rotation = Random.Range(5f, 85f);
            //Debug.Log("rotation " + rotation);
            GameObject[] restOfRotatingParts = new GameObject[numberOfParts - 1];
            for (int i = 0; i < numberOfParts - 1; i++) {
                restOfRotatingParts[i] = rotatingParts[i];
            }
            StartCoroutine(RotateOverSeconds(restOfRotatingParts, rotation, rotatingTime));
        } else {
            rotatingFinished = true;
        }

    }
}
