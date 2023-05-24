using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class cameraFollower : MonoBehaviour {
	Vector3 offset;
	public GameObject spinner;
	private bool follow;
	Vector3 pos;
	void Start () {
		//spinner =  GameObject.FindGameObjectWithTag ("spinner");
		offset = transform.position - spinner.transform.position;

		follow = true;
	}
//	void Update(){
////		if (spinner == null) {
////			try{
////			spinner =  GameObject.FindGameObjectWithTag ("spinner");
////		}
////			catch (Exception e){
////			}
////		}
//	
//	
//	}

	void Update () {
		if (follow) {

			//if (spinner.activeInHierarchy != false) {
			try{
				pos = new Vector3 (spinner.transform.position.x + offset.x, spinner.transform.position.y + offset.y, spinner.transform.position.z + offset.z );
				transform.position = pos;
			}
			catch (Exception e){
			}
			//}

		}
	}
}
