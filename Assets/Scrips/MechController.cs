using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechController : MonoBehaviour {
	public float moveSpeed = 0.5f;
	public float lookSpeed = 2.0f;
	public int health = 100;
	public int shield = 100;
	public int regen = 2;
	public float regenTime = 0.2f;
	public float fireTime = 0.2f;

	public Transform RocketPrefab = null;
	public Transform MissilePrefab = null;
	public Texture2D targetBoxImage = null;

	private float regenTimer = 0.0f;

	private int maxHealth;
	private int maxShield;

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
	private string fireButtonName = "Fire1";
	private float fireCooldown = 0.0f;
	private string MissileButtonName = "FireRight";
	private float missileCooldown = 0.0f;
	private string RocketButtonName = "FireLeft";
	private float rocketCooldown = 0.0f;
	private string PowerButtonName = "Fire3";

	private string Power1ButtonName = "Power1";
	private string Power2ButtonName = "Power2";
	private string Power3ButtonName = "Power3";
	private string Power4ButtonName = "Power4";
	private string Power5ButtonName = "Power5";

	//Sword variables
	private string swordButtonName = "Fire2";
	private float swordCooldown = 0.0f;
	public GameObject swordSlash;
	private float slashCooldown = 4.0f;
	private Animator animator = null;

	public GameObject muzzleFlash;
	public GameObject bulletFlash;
	private int muzzleFlashTimer = -1;
	private int bulletFlashTimer = -1;
	
	private int railgunDamage = 12;
	private int autocannonDamage = 6;
	private int machinegunDamage = 3;
	private float railgunCooldown = 0.5f;
	private float autocannonCooldown = 0.25f;
	private float machinegunCooldown = 0.125f;
	private float heavyRocketCooldown = 1.0f;
	private float heavyMissileCooldown = 1.0f;

	// Use this for initialization
	void Start () {
		controller =	gameObject.GetComponent<CharacterController>();
		boostLeft =		gameObject.transform.Find ("Body").Find ("Boost_Left").gameObject;
		boostRight =	gameObject.transform.Find ("Body").Find ("Boost_Right").gameObject;
		boostTop =		gameObject.transform.Find ("Body").Find ("Boost_Top").gameObject;
		boostBottom =	gameObject.transform.Find ("Body").Find ("Boost_Bottom").gameObject;
		Cursor.visible = false;
		targetEnemies = new List<GameObject> ();

		maxHealth = health;
		maxShield = shield;

		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		regenTimer += Time.deltaTime;
		while (regenTimer >= regenTime) {
			regenTimer -= regenTime;
			if (shield <= (maxShield - regen))
				shield += regen;
		}

		float lookX = lookSpeed * Input.GetAxis ("Mouse X");
		float lookY = lookSpeed * Input.GetAxis ("Mouse Y");
		
		transform.Rotate (lookY, lookX, 0);

		Vector3 forward = transform.TransformDirection (Vector3.forward);
		Vector3 right = transform.TransformDirection (Vector3.right);

		float horiInput = -Input.GetAxisRaw ("Horizontal");
		float vertInput = -Input.GetAxisRaw ("Vertical");
		float speedMod = 1.0f;
		if (Input.GetButton ("Jump"))
			speedMod = 2.0f;
		float horizontal = (moveSpeed * speedMod) * horiInput;
		float vertical = (moveSpeed * speedMod) * vertInput;

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
			if (muzzleFlashTimer == 3) {
				bulletFlash.gameObject.SetActive (true);
			}
			if (muzzleFlashTimer == 0) {
				muzzleFlash.gameObject.SetActive (false);
				bulletFlash.gameObject.SetActive (false);
				muzzleFlashTimer = -1;
			}

		}
		
		fireCooldown -= Time.deltaTime;
		while (fireCooldown < 0.0f) {
			if ((Input.GetButton (fireButtonName) || Input.GetAxis (fireButtonName) > 0)) {
				RaycastHit hit;
				Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
				Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 100.0f, Color.blue, 50.0f);
				if (Physics.Raycast (ray, out hit)) {
					GameObject target = hit.collider.gameObject;
					Vector3 toTarget = Vector3.Normalize (target.transform.position - gameObject.transform.position);
					RaycastHit hit2;
					Ray ray2 = new Ray (gameObject.transform.position + (toTarget * 2), toTarget);
					if (Physics.Raycast (ray2, out hit2)) {
						GameObject hitObject = hit2.collider.gameObject;
						if (hitObject.CompareTag ("Enemy")) {
							hitObject.GetComponent<EnemyController> ().applyDamage (autocannonDamage);
						} else if (hitObject.CompareTag ("Debris")) {
							hitObject.GetComponent<AsteroidController>().applyDamage(autocannonDamage);
						}
					}
				}
				muzzleFlash.gameObject.SetActive (true);
				muzzleFlashTimer = 5;

				fireCooldown += autocannonCooldown;
			} else {
				fireCooldown = 0.0f;
			}
		}
		if (swordCooldown < 2.5f && swordSlash.gameObject.activeSelf == true) {
			swordSlash.gameObject.SetActive(false);

		}

		swordCooldown -= Time.deltaTime;
		while (swordCooldown < 0.0f) {
			if (Input.GetButton (swordButtonName)) {
				swordSlash.gameObject.SetActive (true);
				animator.SetTrigger ("isSwinging");
				swordCooldown += slashCooldown;

			} else {
				swordCooldown = 0.0f;
			}

		}

		rocketCooldown -= Time.deltaTime;
		while (rocketCooldown < 0.0f) {
			if (Input.GetButton (RocketButtonName)) {
				Transform rocket = (Transform)Instantiate (RocketPrefab, gameObject.transform.position, Camera.main.transform.rotation);
				rocketCooldown += heavyRocketCooldown;
			} else {
				rocketCooldown = 0.0f;
			}
		}
		
		missileCooldown -= Time.deltaTime;
		targetEnemies.Clear ();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		Vector3 lookAt = Camera.main.transform.forward;
		float missileBackup = missileCooldown;
		foreach (GameObject enemy in enemies) {
			Vector3 toEnemy = Vector3.Normalize (enemy.transform.position - Camera.main.transform.position);
			if (Vector3.Dot (lookAt, toEnemy) > 0.8) {
				targetEnemies.Add (enemy);
				missileCooldown = missileBackup;
				while (missileCooldown < 0.0f) {
					if (Input.GetButton (MissileButtonName)) {
						Transform missile = (Transform)Instantiate (MissilePrefab, gameObject.transform.position, Camera.main.transform.rotation);
						missile.GetComponent<MissileController> ().Target = enemy.transform;
						missileCooldown += heavyMissileCooldown;
					} else {
						missileCooldown = 0.0f;
					}
				}
			}
		}


		if (Input.GetButtonDown (PowerButtonName)) {
			this.gameObject.GetComponent<AscendPower>().usePower();
		}

		if (Input.GetButtonDown (Power1ButtonName)) {
			this.gameObject.GetComponent<AscendPower>().usePower();
		}
		if (Input.GetButtonDown (Power2ButtonName)) {
			this.gameObject.GetComponent<ShockwavePower>().usePower();
		}
		if (Input.GetButtonDown (Power3ButtonName)) {
			this.gameObject.GetComponent<IntangiblePower>().usePower();
		}
		if (Input.GetButtonDown (Power4ButtonName)) {
			this.gameObject.GetComponent<LifestealPower>().usePower();
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

	public void applyDamage(int damage) {
		shield -= damage;
		if (shield < 0) {
			health += shield;
			if (health < 0) {
				health = 0;
			}
			shield = 0;
		}
	}

	public void restoreHealth(int restore) {
		health += restore;
		if (health > maxHealth)
			health = maxHealth;
	}

	public int getMaxHealth() {
		return maxHealth;
	}

	public int getMaxShield() {
		return maxShield;
	}
}
