using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerControl : MonoBehaviour {

	[SerializeField]List<Transform> targets = new List<Transform>();
	//for Test 
	public GameObject name;
	Vector3 pos;
	GameObject centre;
	[SerializeField] GameObject follower;
	[SerializeField] GameObject spark;
	[SerializeField] int state;
	bool isSparkAvailable;
	[SerializeField]Transform otherToFollow;
	Rigidbody rb;
	Vector3 speed;
	float speeds;
	[SerializeField] float rate = 100f;
	[SerializeField]private bool canControl;
	[SerializeField]private bool followMainBool;
	private bool outStateBool;
	[SerializeField]private bool idleStateBool;
	[SerializeField]private bool followOtherStateBool;
	public int id;
	Transform lastTouched;
	public bool isOut;
	private int currentScale;
	public bool upScale;
	private int whoCollided;
	private int noOfCollision;
	public bool isAvailable;
	private bool startAnimationBool;
	float rotationSpeed;
	int collision;
	void Start () {
		collision = 0;
		centre = GameObject.FindGameObjectWithTag ("centre");
		startAnimationBool = true;
		isAvailable = true;
		noOfCollision = 0;
		currentScale = 0;
		upScale = false;
		id = gameObject.GetComponent<enemyControl> ().getrotatorId ();
		isSparkAvailable = true;
		rb = gameObject.GetComponent<Rigidbody> ();
		isOut = false;
		gameObject.GetComponent<enemyControl> ().setAvailability (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (canControl && idleStateBool && isAvailable) {
			transform.RotateAround (new Vector3 (0, 0, 0), Vector3.up, 1f);
			rotationSpeed = UnityEngine.Random.Range(50f,60f);
			//transform.parent.transform.Rotate (Vector3.up*Time.deltaTime*rotationSpeed);
		} else if (canControl && followOtherStateBool && isAvailable) {
		  // transform.position = Vector3.MoveTowards (transform.position, otherToFollow.transform.position, rate*Time.deltaTime);
			Vector3 direction = otherToFollow.position - transform.position;
			direction = direction.normalized;
//			transform.Translate (direction);
			speeds = UnityEngine.Random.Range(20f,30f);
			rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z),new Vector3(direction.x*speeds,0f,direction.z*speeds),3f*Time.deltaTime);
			//rb.AddForce(direction*speeds,ForceMode.Acceleration);
			if(!otherToFollow.gameObject.GetComponent<playerControl>().isAvailable){
				followOtherStateBool = false;
				StartCoroutine (followOtherState ());
			}
		
		}
		else if (canControl && followMainBool && isAvailable) {
			//transform.position = Vector3.MoveTowards (transform.position, follower.transform.position, rate*Time.deltaTime);
			Vector3 directions = follower.transform.position - transform.position;
			directions = directions.normalized;
			speeds = UnityEngine.Random.Range(20f,30f);
			rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z),new Vector3(directions.x*speeds,0f,directions.z*speeds),3f*Time.deltaTime);
			//rb.AddForce(directions*speeds,ForceMode.Acceleration);

		}
		if (isOut) {
			isOut = false;
			name.SetActive (false);
			outProcedure ();

		}
		if (upScale) {
			upScale = false;
			upScaleRotator ();
		}
		if (startAnimationBool && enemy.startTheGame) {
			Vector3 direction = centre.transform.position - transform.position;
			direction = direction.normalized;
			speeds = 20f;
			rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z),new Vector3(direction.x*speeds,0f,direction.z*speeds),3f*Time.deltaTime);
		}
		pos = new Vector3 (transform.position.x, name.transform.position.y,transform.position.z);
		name.transform.position = pos;
	}
//	void targetSelector(){
//		int index = Random.Range (0, targets.Count + 1);
//	
//	
//	
//	}
	void OnCollisionEnter(Collision other){
		collision++;
		lastTouched = other.transform;
		if (startAnimationBool) {
			startAnimationBool = false;
			canControl = true;
			stateDecider ();
		} else {
			if (other.gameObject.tag == "spinner") {
				canControl = false;
				whoCollided = 1;
				StartCoroutine (counter (other.gameObject));
				Vector3 direction = other.gameObject.transform.position - transform.position;
				direction = -direction.normalized;
				float speeds = other.gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
				//rb.AddForce (direction * 20f,ForceMode.Impulstre);
				rb.AddForce (direction * speeds * forceDecider (other.gameObject), ForceMode.Impulse);
				if (isSparkAvailable) {
					spark.transform.position = other.contacts [0].point;
					spark.transform.rotation = Quaternion.Euler (direction.x, direction.y, direction.z);
					StartCoroutine (sparkManager ());
				}
			}
			if (other.gameObject.tag == "enemy") {
				canControl = false;
				whoCollided = 2;
				StartCoroutine (counter (other.gameObject));
				Vector3 direction = other.gameObject.transform.position - transform.position;
				direction = -direction.normalized;
				float speeds = other.gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
				rb.AddForce (direction * speeds * forceDecider (other.gameObject)*0.8f, ForceMode.Impulse);
				//rb.AddForce (direction, ForceMode.Impulse);
				if (isSparkAvailable) {
					spark.transform.position = other.contacts [0].point;
					spark.transform.rotation = Quaternion.Euler (direction.x, direction.y, direction.z);
					StartCoroutine (sparkManager ());
				}
				if (followOtherStateBool) {
					followOtherStateBool = false;
					stateDecider ();
				}
			}
			if (collision >= 10) {
				upScaleRotator ();
				collision = 0;
			}
		}
	}
	IEnumerator counter(GameObject obj){
		yield return new WaitForSeconds (timeDecider(obj));
		canControl = true;
		rb.velocity = Vector3.zero;
		stateDecider ();
	}
	IEnumerator followOtherState(){
//		int index = (int)Random.Range (0, targets.Count);
//		otherToFollow = targets [index];

		state = 0;
		int index = (int)UnityEngine.Random.Range(0,enemy.activeRotators.Count);
		otherToFollow = enemy.activeRotators [index].transform;
//		if (GameObject.ReferenceEquals( otherToFollow.gameObject, this.gameObject)) {
//			StartCoroutine (idleState ());  
//			yield return null;
//		}
		if (this.id == otherToFollow.gameObject.GetComponent<playerControl> ().id) {
			StartCoroutine (idleState ());  
			yield return null;
		}
			
		followOtherStateBool = true;

		//float timer = Random.Range (2f, 4f);
		//yield return new WaitForSeconds (timer);
		//followOtherStateBool = false;
		//stateDecider ();
		yield return null;
	}
	IEnumerator idleState(){
		// do nothing
		state = 1;
		idleStateBool = true;
		float timer = UnityEngine.Random.Range(5f,7f);
		yield return new WaitForSeconds(timer);
		idleStateBool = false;
		stateDecider ();
	}

	IEnumerator followMain(){
		state = 2;
		followMainBool = true;
		float timer = UnityEngine.Random.Range (7f, 10f);
		yield return new WaitForSeconds (timer);
		followMainBool = false;
		stateDecider ();
	}
	void stateDecider(){
		if (enemy.numberOfPlayersAlive <= 2) {
		
			StartCoroutine (followMain ());
		} else if (enemy.numberOfPlayersAlive <= 8) {
			int val = (int)UnityEngine.Random.Range (0, 2);
			if (val < 1) {
				StartCoroutine (followOtherState ());
			
			} else {
				StartCoroutine (followMain ());
			}
		    
		} else {
			if (this.gameObject.GetComponent<enemyControl> ().getTypeOfRotator () == 1) {
				int prediction = (int)UnityEngine.Random.Range (0, 10);
				if (prediction <= 3) {
					StartCoroutine (followMain ());
				} else {
					StartCoroutine (followOtherState ());
				}
			} else {
				int predictor = (int)UnityEngine.Random.Range (0, 10f);
				if (predictor <= 2 && predictor >= 0) {
					StartCoroutine (followMain ());
				} else if (predictor <= 8 && predictor > 2) {
					StartCoroutine (followOtherState ());
				} else if (predictor <= 9) {
					StartCoroutine (idleState ());
				}
//			} else if (predictor <= 10 && predictor > 8) {
//				StartCoroutine (outState ());
//			} 
			}
		}
	}
	IEnumerator sparkManager(){
		isSparkAvailable = false;
		spark.SetActive (true);
		//spark.GetComponent<particl
		yield return new WaitForSeconds(0.2f); 
		spark.GetComponent<ParticleSystem> ().Stop ();
		yield return new WaitForSeconds (0.5f);
		spark.SetActive (false);
		isSparkAvailable = true;
	}
	void outProcedure(){
	//scale up the last touched
		enemy.activeRotators.Remove(this.gameObject);

		canControl = false;
		isAvailable = false;
		try{
		if (whoCollided == 1) {
			if (lastTouched != null)
				lastTouched.gameObject.GetComponent<control> ().upScale = true;
		} else if (whoCollided == 2) {
			lastTouched.gameObject.GetComponent<playerControl> ().upScale = true;
		
		}
			enemy.numberOfPlayersAlive--;
		
		rb.mass = 50f;
		rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
		rb.constraints = RigidbodyConstraints.None;
		rb.useGravity = true;
		
        
		}
		catch (Exception e){
		}

	}
	public void upScaleRotator(){
		float x = transform.localScale.x;
		x += 0.2f;
		float y = x * 1.5f;
		Vector3 scale = new Vector3 (x, y, x);
		transform.localScale = scale;
	
	}
	public float forceDecider(GameObject obj){
	
		if (obj.transform.localScale.x >= transform.localScale.x) {

			float ret = 1f + 1.5f * (obj.transform.localScale.x - transform.localScale.x);
			return ret;
		} else {
			float ret = 1f - ( transform.localScale.x - obj.transform.localScale.x ); 
			if (ret > 0) {
				return ret;
			} else {
				return 0.2f;
			}

		}
	
	
	}
	void startAnimation(){

               
	}
	public float timeDecider(GameObject obj){
		if (obj.transform.localScale.x >= transform.localScale.x) {
		     
			float ret = 1f + 1.5f * (obj.transform.localScale.x - transform.localScale.x);
			return ret;
		} else {
			float ret = 1f - ( transform.localScale.x - obj.transform.localScale.x ); 
			if (ret > 0) {
				return ret;
			} else {
				return 0.2f;
			}
		
		}
	
	}

}
