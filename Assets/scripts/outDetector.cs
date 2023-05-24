using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outDetector : MonoBehaviour {

	void OnTriggerExit(Collider other){
	
		if (other.tag == "enemy") {
			
				other.GetComponent<playerControl> ().isOut = true;

		
		
		} else if (other.tag == "spinner") {
		//game over
			print("OUT");
		}
	
	
	
	}
}
