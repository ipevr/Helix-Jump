using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Input.mousePosition.x;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
