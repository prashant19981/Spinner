using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enemy : MonoBehaviour {
	[SerializeField]public static List<GameObject> activeRotators = new List<GameObject>();
	public static int numberOfPlayersAlive;
	[SerializeField]List<Sprite> stadium;
	[SerializeField]SpriteRenderer stadiumImage;
	[SerializeField]Transform parent;
	[SerializeField]Transform pos;
	[SerializeField]Transform names;
	[SerializeField]List<string> users;
	[SerializeField] GameObject startButton;
	public static bool startTheGame = false;
	[SerializeField]GameObject uiText;
	[SerializeField]GameObject speedManager;
	[SerializeField]GameObject settingButton;
	[SerializeField]GameObject coinImage;
	[SerializeField]GameObject skins;
	[SerializeField]GameObject noAdsButton;
	[SerializeField]GameObject vipButton;
	[SerializeField]GameObject gameName;
	Text usersJoined;
	float timer;
	void Awake(){
		Time.timeScale = 0f;
		startTheGame = false;
		stadiumImageSetter ();
	}
	void Start(){
		usersJoined = uiText.transform.GetChild (0).gameObject.GetComponent<Text> ();
	}
	IEnumerator initEnemySelection(){
		yield return new WaitForSeconds (0.1f);
		int count = 0;
		while (count < 11) {
			int index = (int)Random.Range (0, parent.childCount);
			Transform obj = parent.GetChild (index);
			if (obj.gameObject.activeInHierarchy == false) {
				obj.GetChild (0).position = pos.GetChild (count).position;
				obj.gameObject.SetActive (true);
				names.GetChild (index).position = new Vector3 (obj.position.x, names.GetChild (index).position.y, obj.position.z);
				int userIndex = (int)UnityEngine.Random.Range (0, users.Count);
				names.GetChild (index).gameObject.GetComponent<Text> ().text = users [userIndex];
				users.RemoveAt (userIndex);
				usersJoined.text = "Finding players..";
				activeRotators.Add (obj.transform.GetChild(0).gameObject);
				count++;
				names.GetChild (index).gameObject.SetActive (true);
				timer = UnityEngine.Random.Range (0.5f, 1f);
				yield return new WaitForSeconds (timer);
			}
		}
		numberOfPlayersAlive = 7;
		assignType ();
		StartCoroutine (afterJoined ());
	}
	void assignType(){
		int index = Random.Range (0, 8);
		activeRotators [index].GetComponent<enemyControl> ().setTypeOfRotator (1);
		index = Random.Range (0, 8);
		activeRotators [index].GetComponent<enemyControl> ().setTypeOfRotator (1);
	}
	public void startMatch(){
		startButton.SetActive (false);
		settingButton.SetActive (false);
		coinImage.SetActive (false);
		skins.SetActive (false);
		noAdsButton.SetActive (false);
		vipButton.SetActive (false);
		gameName.SetActive (false);
		uiText.SetActive (true);
		Time.timeScale = 1f;
		StartCoroutine(initEnemySelection ());
	}
	IEnumerator afterJoined(){
		yield return new WaitForSeconds (0.5f);
		usersJoined.text = "3";
		yield return new WaitForSeconds (0.5f);
		usersJoined.text = "2";
		yield return new WaitForSeconds (0.5f);
		usersJoined.text = "1";
		yield return new WaitForSeconds (0.5f);
		usersJoined.text = "Go..!";
		yield return new WaitForSeconds (0.5f);
		speedManager.SetActive (true);
		uiText.SetActive (false);

		startTheGame = true;
	
	}
	void stadiumImageSetter(){
		int index = (int)UnityEngine.Random.Range (0, stadium.Count);
		stadiumImage.sprite = stadium [index];
	}

}
