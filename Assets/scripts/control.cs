using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control : MonoBehaviour {
	GameObject centre;
	[SerializeField] GameObject spinner;
	Rigidbody rb;
	Vector3 startMousePos;
	Vector2 maxDifference = new Vector2 (50f, 50f);
	Vector3 currentMousePos;
	float speed;
	private bool canControl;
	[SerializeField] GameObject spark;
	bool isSparkAvailable;
	public bool upScale;
	public bool isOut;
	int collision;
	bool startAnimation;
	[SerializeField] GameObject speedAvailaber;
	private bool speedMeterIncrease;
	void Awake(){
		speedMeterIncrease = true;
		isOut = false;
		Application.targetFrameRate = 60;
	}
	void Start () {
		collision = 0;
		centre = GameObject.FindGameObjectWithTag ("centre");
		startAnimation = true;
		upScale = false;
		rb = spinner.GetComponent<Rigidbody> ();
		isSparkAvailable = true;

	}
	void Update () {
		print (speed);
		if (Input.GetMouseButtonDown (0)) {
			startMousePos = Input.mousePosition;
			if (speedMeterIncrease) {
				speed = 15f;
			} else {
				speed = 21f;
			}
		}
		if (Input.GetMouseButton (0)) {
			if (speedMeterIncrease) {
				speed = 15f;
			} else {
				speed = 21f;
			}
			currentMousePos = Input.mousePosition;
			if(canControl)
				movement ();
		
		
		}
		if (upScale) {
			upScale = false;
			upScaleRotator ();
		}
		if (isOut) {
			isOut = false;
			outProcedure ();
		}
		if (startAnimation && enemy.startTheGame) {
			Vector3 direction = centre.transform.position - transform.position;
			direction = direction.normalized;
			float speeds = 20f;
			rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z),new Vector3(direction.x*speeds,0f,direction.z*speeds),3f*Time.deltaTime);
		}
		if (enemy.startTheGame) {
			if (speedMeterIncrease) {
				if (speedAvailaber.GetComponent<Image> ().fillAmount < 1f) {
					speedAvailaber.GetComponent<Image> ().fillAmount += 0.15f * Time.deltaTime;
				} else {
					speedMeterIncrease = false;
			
				}
			} else {
				if (speedAvailaber.GetComponent<Image> ().fillAmount > 0f && Input.GetMouseButton (0)) {
					speedAvailaber.GetComponent<Image> ().fillAmount -= 0.2f * Time.deltaTime;
				} else {
					speedMeterIncrease = true;
				}
			}
		}
	}
	void movement(){
		Vector2 difference = new Vector2 (Mathf.Abs (currentMousePos.x) - Mathf.Abs (startMousePos.x), Mathf.Abs (currentMousePos.y) - Mathf.Abs (startMousePos.y));
		if (Mathf.Abs (difference.x) > maxDifference.x) {
			difference.x = (difference.x / Mathf.Abs (difference.x));
		} else {
			difference.x = (difference.x / maxDifference.x);
		}
		if (Mathf.Abs (difference.y) > maxDifference.y) {
			difference.y = (difference.y / Mathf.Abs (difference.y));
		} else {
			difference.y = (difference.y / maxDifference.y);
		}
		//rb.AddForce(new Vector3(difference.x*speed,0f,difference.y*speed),ForceMode.Acceleration);
		//transform.Translate(Vector3.Lerp( ,new Vector3(difference.x*speed*Time.deltaTime,0f,difference.y*speed*Time.deltaTime));

		rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z),new Vector3(difference.x*speed,0f,difference.y*speed),speed*Time.deltaTime);
	}
	void OnCollisionEnter(Collision other){
		collision++;

		if (startAnimation) {
			startAnimation = false;
			canControl = true;
		}
		canControl = false;
		StartCoroutine (counter (other.gameObject));
		Vector3 direction =  other.gameObject.transform.position - transform.position ;
		if (isSparkAvailable) {
			spark.transform.position = other.contacts [0].point;
			spark.transform.rotation = Quaternion.Euler( direction.x,direction.y,direction.z);
			StartCoroutine (sparkManager ());
		}
		direction = -direction.normalized;
		float speeds = other.gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
		rb.AddForce (direction * speeds*forceDecider(other.gameObject),ForceMode.Impulse);
		if (collision >= 15) {
			upScaleRotator ();
			collision = 0;
		}
	}
	IEnumerator counter(GameObject obj){
		yield return new WaitForSeconds (timeDecider(obj));
		canControl = true;
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
	public void upScaleRotator(){
		float x = transform.localScale.x;
		x += 0.2f;
		float y = x * 1.5f;
		Vector3 scale = new Vector3 (x, y, x);
		transform.localScale = scale;
		collision = 0;

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

	public void outProcedure(){
		canControl = false;
		rb.mass = 50f;
		rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
		rb.constraints = RigidbodyConstraints.None;
		rb.useGravity = true;
	}
	public float timeDecider(GameObject obj){
		if (obj.transform.localScale.x >= transform.localScale.x) {

			float ret = 0.2f + (obj.transform.localScale.x - transform.localScale.x);
			return ret;
		} else {
			float ret = 0.2f - 0.1f*(transform.localScale.x - obj.transform.localScale.x ); 
			if (ret > 0) {
				return ret;
			} else {
				return 0.2f;
			}

		}

	}
	void speedManagerIncrease(){
		


	}
}
