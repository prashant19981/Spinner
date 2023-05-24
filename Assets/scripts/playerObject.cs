using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerObject : NetworkBehaviour {

	public GameObject spinner;
	void Start () {
	
		if (hasAuthority == false) {
		
			return;
		}
	//	Instantiate (spinner);
		CmdspawnSpinner();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Command]
	void CmdspawnSpinner (){
		GameObject obj = Instantiate (spinner);
		Camera.main.GetComponent<cameraFollower> ().spinner = obj;
		NetworkServer.Spawn (obj);
	}
}
