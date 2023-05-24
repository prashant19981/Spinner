using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward*Time.deltaTime*1000f);
		//gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up);

	}
}
