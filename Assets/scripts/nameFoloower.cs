using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class nameFoloower : MonoBehaviour {
	public GameObject spinner;
	Vector3 pos;
	public bool isOut;
	void Update () {
		if (!isOut) {
			try {
				pos = new Vector3 (spinner.transform.position.x, transform.position.y, spinner.transform.position.z);
				transform.position = pos;
			} catch (Exception e) {
			}
		} else {
			gameObject.SetActive (false);
		
		}	//}


	}
}
