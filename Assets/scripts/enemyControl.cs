using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyControl: MonoBehaviour {
	private bool isAvailable;
	[SerializeField]private int rotatorId;
	private int typeOfRotator;
	public void setAvailability(bool val){
		this.isAvailable = val;
	}
	public bool getAvailability(){
		return this.isAvailable;
	}
	public int getrotatorId(){
		return this.rotatorId;
	}
	public void setTypeOfRotator(int value){
		this.typeOfRotator = value;
	}
	public int getTypeOfRotator(){
		return this.typeOfRotator;
	}


}
