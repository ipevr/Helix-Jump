using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressController : MonoBehaviour {

    RawImage rawImage;
    float levelProgress = 0f;
    float maxProgress = 0.5f;

	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
        rawImage.uvRect = new Rect(levelProgress, 0f, 0.5f, 1f);
	}
	
    public void ShowLevelProgress(float progressInPercent) {
        float progress = (progressInPercent / 100) * maxProgress;
        levelProgress += progress;
        rawImage.uvRect = new Rect(levelProgress, 0f, 0.5f, 1f);
    }

}
