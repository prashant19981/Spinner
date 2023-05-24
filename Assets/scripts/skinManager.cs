using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skinManager : MonoBehaviour {


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void openShop(){
		gameObject.GetComponent<Animator> ().SetTrigger ("shop");
	}
}
