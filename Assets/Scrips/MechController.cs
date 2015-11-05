using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechController : MonoBehaviour {
	public float moveSpeed = 0.5f;
	public float lookSpeed = 2.0f;
	
	public Transform RocketPrefab = null;
	public Transform MissilePrefab = null;
	public Texture2D targetBoxImage = null;

	private CharacterController controller;
	private GameObject boostLeft;
	private GameObject boostRight;
	private GameObject boostTop;
	private GameObject boostBottom;
	
	private Vector3 moveDir = Vector3.zero;

	private List<GameObject> targetEnemies = null;

	private float targetBoxWidth = 256;
	private float targetBoxHeight = 256;

	//Raycasting Variables
	public string fireButtonName = "Fire1";
	private bool readyToFire = true;
	public string MissileButtonName = "FireRight";
	private bool readyToMissile = true;
<<<<<<< HEAD
	public GameObject muzzleFlash;
	private int muzzleFlashTimer = -1;


=======
	public string RocketButtonName = "FireLeft";
	private bool readyToRocket = true;
>>>>>>> 4b80ba2ee3f2a8b7a4a9a795c4833a3482604b90

	// Use this for initialization
	void Start () {
		controller =	gameObject.GetComponent<CharacterController>();
		boostLeft =		gameObject.transform.Find ("Body").Find ("Boost_Left").gameObject;
		boostRight =	gameObject.transform.Find ("Body").Find ("Boost_Right").gameObject;
		boostTop =		gameObject.transform.Find ("Body").Find ("Boost_Top").gameObject;
		boostBottom =	gameObject.transform.Find ("Body").Find ("Boost_Bottom").gameObject;
		Cursor.visible = false;
		targetEnemies = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		float lookX = lookSpeed * Input.GetAxis ("Mouse X");
		float lookY = lookSpeed * Input.GetAxis ("Mouse Y");
		
		transform.Rotate (lookY, lookX, 0);

		Vector3 forward =	transform.TransformDirection(Vector3.forward);
		Vector3 right =		transform.TransformDirection (Vector3.right);

		float horiInput = -Input.GetAxisRaw ("Horizontal");
		float vertInput = -Input.GetAxisRaw ("Vertical");
		float speedMod = 1.0f;
		if (Input.GetButton ("Jump"))
			speedMod = 2.0f;
		float horizontal =	(moveSpeed * speedMod) * horiInput;
		float vertical =	(moveSpeed * speedMod) * vertInput;

		moveDir = (vertical * forward) + (horizontal * right);
		controller.Move (moveDir);

		if (horiInput < 0)
			boostLeft.SetActive (true);
		else
			boostLeft.SetActive (false);

		if (horiInput > 0)
			boostRight.SetActive (true);
		else 
			boostRight.SetActive (false);

		if (vertInput < 0) {
			boostTop.SetActive (true);
			boostBottom.SetActive (true);
		} else {
			boostTop.SetActive (false);
			boostBottom.SetActive (false);
		}
		if (muzzleFlashTimer > 0) {
			muzzleFlashTimer--;
			if(muzzleFlashTimer == 0){
				muzzleFlash.gameObject.SetActive (false);
				muzzleFlashTimer = -1;
			}

		}

		if (Input.GetButtonDown (fireButtonName) && readyToFire) 
		{
			RaycastHit hit;
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100.0f, Color.blue, 50.0f);
			if(Physics.Raycast (ray, out hit))
			{
				if(hit.collider.gameObject.CompareTag("Enemy"))
				{
					SendMessageUpwards ("hitScanHit", hit.collider.gameObject);
				}
			}
			muzzleFlash.gameObject.SetActive(true);
			muzzleFlashTimer = 5;
		}

		if (Input.GetButtonDown (RocketButtonName) && readyToRocket) {
			Transform rocket = (Transform) Instantiate(RocketPrefab, gameObject.transform.position, Quaternion.Inverse(gameObject.transform.rotation));
		}

		targetEnemies.Clear ();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Vector3 lookAt = Camera.main.transform.forward;
		foreach (GameObject enemy in enemies) {
			Vector3 toEnemy = Vector3.Normalize(enemy.transform.position - Camera.main.transform.position);
			if (Vector3.Dot(lookAt, toEnemy) > 0.8) {
				targetEnemies.Add(enemy);
				if (Input.GetButtonDown (MissileButtonName) && readyToMissile) {
					Transform missile = (Transform) Instantiate(MissilePrefab, gameObject.transform.position, Quaternion.Inverse(gameObject.transform.rotation));
					missile.GetComponent<MissileController>().Target = enemy.transform;
				}
			}
		}
	}

	void OnGUI() {
		foreach (GameObject enemy in targetEnemies) {
			if (enemy == null) continue;
			Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
			float distance = Vector3.Distance(enemy.transform.position, Camera.main.transform.position);
			if (distance > 0) {
				float width = targetBoxWidth / distance * 10;
				float height = targetBoxHeight / distance * 10;
				float boxX = screenPos.x - (width / 2);
				float boxY = Screen.height - screenPos.y - (height / 2);

				GUI.DrawTexture(new Rect(boxX, boxY, width, height), targetBoxImage);
			}
		}
	}
}
