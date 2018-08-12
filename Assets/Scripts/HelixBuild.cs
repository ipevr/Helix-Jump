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

    float positionOfGap;
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
            CreateTiltParts(3);
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
        positionOfGap = Random.Range(0f, 360f);
        //positionOfGap = 0f;
        print("positionOfGap1 " + positionOfGap);
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
        //float randomRotationOffset = Random.Range(90f, 180f);
        float randomRotationOffset = 95f;
        float rotation = positionOfGap + randomRotationOffset;
        for (int i = 0; i < number; i++) {
            myTiltParts[i] = Instantiate(tiltPartPrefab, platform.transform);
        }
        StartCoroutine(RotateOverSeconds(myTiltParts, rotation, buildTime));

        //randomRotationOffset = Random.Range(0f, 90f);
        randomRotationOffset = 90f;
        rotation += randomRotationOffset;
        
    }

    void CreatePlatformExitObserver() {
        GameObject myPlatformExitObserver = Instantiate(platformExitObserverPrefab, platform.transform);
    }

    void CreateBottom() {
        CreatePlatformGameObject();
        bottom = Instantiate(bottomPrefab, platform.transform);
    }

    IEnumerator RotateOverSeconds(GameObject[] rotatingParts, float rotation, float rotatingTotalTime) {
        float elaspedTime = 0f;
        float rotatingTime = rotatingTotalTime / rotatingParts.Length;
        int i = rotatingParts.Length;
        // store the start positions of each rotatinPart
        Quaternion[] startPosition = new Quaternion[i];
        for (int j = 0; j < i; j++) {
            startPosition[j] = rotatingParts[j].transform.rotation;
        }
        // rotate all parts over time
        while (elaspedTime < rotatingTime) {
            for (int j = 0; j < i; j++) {
                rotatingParts[j].transform.rotation = Quaternion.Slerp(startPosition[j], Quaternion.AngleAxis(rotation, Vector3.up), (elaspedTime / rotatingTime));
            }
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // reduce parts by 1 and call the function again for the rest of the parts until there are no parts left
        if (i > 1) {
            rotation += 90f;
            GameObject[] restOfRotatingParts = new GameObject[i - 1];
            for (int j = 0; j < i - 1; j++) {
                restOfRotatingParts[j] = rotatingParts[j];
            }
            StartCoroutine(RotateOverSeconds(restOfRotatingParts, rotation, rotatingTime));
        } else {
            rotatingFinished = true;
        }

    }
}
