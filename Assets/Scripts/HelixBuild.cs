using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixBuild : MonoBehaviour {

    [SerializeField]
    GameObject helix;

    [SerializeField]
    GameObject normalBounncingPart;

    [SerializeField]
    GameObject tiltPart;

    [SerializeField]
    GameObject bottom;

    [SerializeField]
    GameObject platformExitObserver;

    float positionOfGap;
    float yPosition = 0f;
    GameObject platform;

	// Use this for initialization
	void Start () {
		
	}
	
    public void CreatePlatforms(int number) {
        for (int i = 0; i < number; i++) {
            CreatePlatformGameObject();
            DefinePositionOfGap();
            CreateNormalBouncingParts();
            CreateTiltParts();
            CreatePlatformExitObserver();
        }
        CreateBottom();
    }

    void CreatePlatformGameObject() {
        platform = new GameObject("Platform");
        platform.transform.parent = helix.transform;
        platform.transform.position = new Vector3(0f, yPosition, 0f);
        yPosition -= 4f;
    }

    void DefinePositionOfGap() {
        positionOfGap = Random.Range(0f, 360f);
    }

    void CreateNormalBouncingParts() {
        float rotation = positionOfGap;
        for (int i = 0; i < 3; i++) {
            GameObject bouncingPart = Instantiate(normalBounncingPart, platform.transform);
            bouncingPart.transform.eulerAngles = new Vector3(0f, rotation, 0f);
            rotation += 90f;
        }
    }

    void CreateTiltParts() {
        float randomRotationOffset = Random.Range(45f, 90f);
        float rotation = positionOfGap + randomRotationOffset;
        for (int i = 0; i < 3; i++) {
            GameObject myTiltPart = Instantiate(tiltPart, platform.transform);
            tiltPart.transform.eulerAngles = new Vector3(0f, rotation, 0f);
            randomRotationOffset = Random.Range(0f, 90f);
            rotation += randomRotationOffset;
        }
    }

    void CreatePlatformExitObserver() {
        GameObject myPlatformExitObserver = Instantiate(platformExitObserver, platform.transform);
    }

    void CreateBottom() {
        CreatePlatformGameObject();
        GameObject myBottom = Instantiate(bottom, platform.transform);
    }
}
