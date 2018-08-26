using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string ACTUAL_LEVEL_KEY = "actual_level";

    public static void SetActualLevel (int level) {
        PlayerPrefs.SetInt(ACTUAL_LEVEL_KEY, level);
    }

    public static int GetActualLevel() {
        if (!PlayerPrefs.HasKey(ACTUAL_LEVEL_KEY)) {
            SetActualLevel(0);
        }
        return PlayerPrefs.GetInt(ACTUAL_LEVEL_KEY);
    }

}
