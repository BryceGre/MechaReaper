using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MechController : MonoBehaviour {
	public const float TargetDot = 0.9f;

	public RectTransform TargetCircle;
	public GUIController GUI = null;
	private float lastScreenWidth;

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

	private int souls;

	private float regenTimer = 0.0f;

	private int maxHealth;
	private int maxShield;

	private CharacterController controller;
	private GameObject boostLeft;
	private GameObject boostRight;
	
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
	private GameObject swordSlash;
	private float slashCooldown = 0.0f;
	private Animator animator = null;
	private Transform reaper;

	public GameObject normalSwordSlash;
	public GameObject longSwordSlash;
	public GameObject daggerSlash;

	private float daggerCooldown = 1.0f;
	private float normalSwordCooldown = 3.0f;
	private float longSwordCooldown = 6.0f;


	public GameObject autoCannonMuzzleFlash;
	public GameObject autoCannonBulletFlash;
	public GameObject railGunMuzzleFlash;
	public GameObject railGunBulletFlash;
	public GameObject machineGunMuzzleFlash;
	public GameObject machineGunBulletFlash;
	private GameObject muzzleFlash;
	private GameObject bulletFlash;

	private int muzzleFlashTimer = -1;
	
	private int railgunDamage = 12;
	private int autocannonDamage = 6;
	private int machinegunDamage = 3;
	private float railgunCooldown = 0.5f;
	private float autocannonCooldown = 0.25f;
	private float machinegunCooldown = 0.125f;
	private float heavyRocketCooldown = 1.0f;
	private float heavyMissileCooldown = 1.0f;
	private int missiles = 10;

	private int gunDamage = 0;
	private float gunCooldown = 0;
	private float normalSwordAnimatorSpeed = 1.0f;
	private float longSwordAnimatorSpeed = 0.5f;
	private float daggerSwordAnimatorSpeed = 2.0f;

	private float swordAnimatorSpeed;
	

	// Use this for initialization
	void Start () {
		controller =	gameObject.GetComponent<CharacterController>();
		souls =			0;
		reaper =		gameObject.transform.Find ("Reaper");
		boostLeft =		reaper.Find ("Body").Find ("Boost_Left").gameObject;
		boostRight =	reaper.Find ("Body").Find ("Boost_Right").gameObject;
		Cursor.visible = false;
		targetEnemies = new List<GameObject> ();

		maxHealth = health;
		maxShield = shield;

		animator = reaper.GetComponent<Animator>();
		lastScreenWidth = 0.0f;

		Color c = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		this.gameObject.GetComponent<ShockwavePower> ().Icon.GetComponent<RawImage> ().color = c;
		this.gameObject.GetComponent<IntangiblePower> ().Icon.GetComponent<RawImage> ().color = c;
		this.gameObject.GetComponent<LifestealPower> ().Icon.GetComponent<RawImage> ().color = c;
		this.gameObject.GetComponent<CataclysmPower> ().Icon.GetComponent<RawImage> ().color = c;
		
		GUI.tooltips[0].gameObject.SetActive(true);
		GUI.tooltip.gameObject.SetActive (true);

		int gunID = PlayerPrefs.GetInt ("gun");
		if (gunID == 0) {
			muzzleFlash = autoCannonMuzzleFlash;
			bulletFlash = autoCannonBulletFlash;
			gunDamage = autocannonDamage;
			gunCooldown = autocannonCooldown;

		} else if (gunID == 1) {
			muzzleFlash = railGunMuzzleFlash;
			bulletFlash = railGunBulletFlash;
			gunDamage = railgunDamage;
			gunCooldown = railgunCooldown;

		} else if (gunID == 2) {
			muzzleFlash = machineGunMuzzleFlash;
			bulletFlash = machineGunBulletFlash;
			gunDamage = machinegunDamage;
			gunCooldown = machinegunCooldown;

		}

		int swordID = PlayerPrefs.GetInt ("melee");
		if (swordID == 0) {
			swordSlash = normalSwordSlash;
			slashCooldown = normalSwordCooldown;
			swordAnimatorSpeed = normalSwordAnimatorSpeed;

		} else if (swordID == 1) {
			swordSlash = longSwordSlash;
			slashCooldown = longSwordCooldown;
			swordAnimatorSpeed = longSwordAnimatorSpeed;

		} else if (swordID == 2) {
			swordSlash = daggerSlash;
			slashCooldown = daggerCooldown;
			swordAnimatorSpeed = daggerSwordAnimatorSpeed;
		}


	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0.0f)
			return;

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
			speedMod = 1.5f;
		float horizontal = (moveSpeed * speedMod) * horiInput;
		float vertical = (moveSpeed * speedMod) * vertInput;

		moveDir = (vertical * forward) + (horizontal * right);
		controller.Move (moveDir);

		//Vector3 newUp = transform.up + (transform.forward * (vertInput * 0.25f)) + (transform.right * (horiInput * 0.25f));
		//Vector3 newForward = transform.forward - (transform.up * (vertInput * 0.25f));
		//reaper.rotation = Quaternion.LookRotation(Vector3.Lerp (reaper.forward, newForward, Time.deltaTime), Vector3.Lerp (reaper.up, newUp, Time.deltaTime));
		Vector3 newUp = transform.up + (transform.right * (horiInput * 0.25f));
		reaper.rotation = Quaternion.LookRotation(transform.forward, Vector3.Lerp (reaper.up, newUp, Time.deltaTime));

		if (horiInput < 0)
			boostLeft.SetActive (true);
		else
			boostLeft.SetActive (false);

		if (horiInput > 0)
			boostRight.SetActive (true);
		else 
			boostRight.SetActive (false);

		if (vertInput != 0) {
			//boostBottom.SetActive (true);
		} else {
			//boostBottom.SetActive (false);
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
							hitObject.GetComponent<EnemyController> ().applyDamage (gunDamage);
						} else if (hitObject.CompareTag ("Debris")) {
							hitObject.GetComponent<AsteroidController>().applyDamage(gunDamage);
						}
					}
				}
				muzzleFlash.gameObject.SetActive (true);
				muzzleFlashTimer = 5;

				fireCooldown += gunCooldown;
			} else {
				fireCooldown = 0.0f;
			}
		}
		swordCooldown -= Time.deltaTime;
		if (swordCooldown < (slashCooldown/1.5) && swordSlash.gameObject.activeSelf == true) {
			swordSlash.gameObject.SetActive(false);

		}
		while (swordCooldown < 0.0f) {
			if (Input.GetButton (swordButtonName)) {
				swordSlash.gameObject.SetActive (true);
				animator.speed = swordAnimatorSpeed;
				animator.SetTrigger ("isSwinging");
				swordCooldown += slashCooldown;

			} else {
				swordCooldown = 0.0f;
			}

		}

		rocketCooldown -= Time.deltaTime;
		while (rocketCooldown < 0.0f) {
			if (Input.GetButtonDown (RocketButtonName)) {
				Instantiate (RocketPrefab, gameObject.transform.position, Camera.main.transform.rotation);
				rocketCooldown += heavyRocketCooldown;
			} else {
				rocketCooldown = 0.0f;
			}
		}
		
		missileCooldown -= Time.deltaTime;
		while (missileCooldown < 0.0f) {
			missiles++;
			missileCooldown += heavyMissileCooldown;
		}
		targetEnemies.Clear ();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		Vector3 lookAt = Camera.main.transform.forward;
		foreach (GameObject enemy in enemies) {
			Vector3 toEnemy = Vector3.Normalize (enemy.transform.position - Camera.main.transform.position);
			if (Vector3.Dot (lookAt, toEnemy) > TargetDot) {
				targetEnemies.Add (enemy);
				if (Input.GetButtonDown (MissileButtonName) && missiles > 0) {
					Transform missile = (Transform)Instantiate (MissilePrefab, gameObject.transform.position, Camera.main.transform.rotation);
					missile.GetComponent<MissileController> ().Target = enemy.transform;
					missiles--;
					GUI.tooltip.gameObject.SetActive (false);
				}
			}
		}

		if (Input.GetButtonDown (PowerButtonName)) {
			this.gameObject.GetComponent<AscendPower>().usePower();
		}
		if (Input.GetButtonDown (Power1ButtonName)) {
			this.gameObject.GetComponent<AscendPower>().usePower();
			GUI.tooltips[0].gameObject.SetActive(false);
		}
		if (Input.GetButtonDown (Power2ButtonName) && souls >= 100) {
			this.gameObject.GetComponent<ShockwavePower>().usePower();
			GUI.tooltips[1].gameObject.SetActive(false);
		}
		if (Input.GetButtonDown (Power3ButtonName) && souls >= 200) {
			this.gameObject.GetComponent<IntangiblePower>().usePower();
			GUI.tooltips[2].gameObject.SetActive(false);
		}
		if (Input.GetButtonDown (Power4ButtonName) && souls >= 300) {
			this.gameObject.GetComponent<LifestealPower>().usePower();
			GUI.tooltips[3].gameObject.SetActive(false);
		}
		if (Input.GetButtonDown (Power5ButtonName) && souls >= 400) {
			this.gameObject.GetComponent<CataclysmPower>().usePower();
			GUI.tooltips[4].gameObject.SetActive(false);
		}
	}

	void OnGUI() {
		GUI.missiles.text = missiles.ToString ("D4");
		GUI.souls.text = souls.ToString("D4");

		foreach (GameObject enemy in targetEnemies) {
			if (enemy == null) continue;
			Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
			float distance = Vector3.Distance(enemy.transform.position, Camera.main.transform.position);
			if (distance > 0) {
				float width = targetBoxWidth / distance * 10;
				float height = targetBoxHeight / distance * 10;
				float boxX = screenPos.x - (width / 2);
				float boxY = Screen.height - screenPos.y - (height / 2);

				UnityEngine.GUI.DrawTexture(new Rect(boxX, boxY, width, height), targetBoxImage);
			}
		}

		if (lastScreenWidth != Screen.width) {
			lastScreenWidth = Screen.width;

			//calculate circle for GUI targeting
			float angle = Mathf.Acos (TargetDot);
			float radius = 10.0f * Mathf.Tan (angle);
			Vector3 front = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 10.0f));
			Vector3 right = front + (Camera.main.transform.right * radius);
			Vector3 top = front + (Camera.main.transform.up * radius);
			Vector3 left = front - (Camera.main.transform.right * radius);
			Vector3 bottom = front - (Camera.main.transform.up * radius);
			
			RectTransform rect = GUI.GUICanvas.GetComponent<RectTransform> ();
			
			Vector2 viewRight = Camera.main.WorldToViewportPoint (right);
			Vector2 viewTop = Camera.main.WorldToViewportPoint (top);
			Vector2 viewLeft = Camera.main.WorldToViewportPoint (left);
			Vector2 viewBottom = Camera.main.WorldToViewportPoint (bottom);
			
			float rightX = (viewRight.x * rect.sizeDelta.x) - (rect.sizeDelta.x * 0.5f);
			float leftX = (viewLeft.x * rect.sizeDelta.x) - (rect.sizeDelta.x * 0.5f);
			float topY = (viewTop.y * rect.sizeDelta.y) - (rect.sizeDelta.y * 0.5f);
			float botY = (viewBottom.y * rect.sizeDelta.y) - (rect.sizeDelta.y * 0.5f);
			TargetCircle.sizeDelta = new Vector2 (rightX - leftX, topY - botY);
		}
	}
	
	
	public void incrementSoulScore()
	{
		souls++;
		if (souls == 100) {
			Color c = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			this.gameObject.GetComponent<ShockwavePower> ().Icon.GetComponent<RawImage> ().color = c;
			GUI.tooltips[0].gameObject.SetActive(false);
			GUI.tooltips[1].gameObject.SetActive(true);
		} else if (souls == 200) {
			Color c = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			this.gameObject.GetComponent<IntangiblePower> ().Icon.GetComponent<RawImage> ().color = c;
			GUI.tooltips[1].gameObject.SetActive(false);
			GUI.tooltips[2].gameObject.SetActive(true);
		} else if (souls == 300) {
			Color c = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			this.gameObject.GetComponent<LifestealPower> ().Icon.GetComponent<RawImage> ().color = c;
			GUI.tooltips[2].gameObject.SetActive(false);
			GUI.tooltips[3].gameObject.SetActive(true);
		} else if (souls == 400) {
			Color c = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			this.gameObject.GetComponent<CataclysmPower> ().Icon.GetComponent<RawImage> ().color = c;
			GUI.tooltips[3].gameObject.SetActive(false);
			GUI.tooltips[4].gameObject.SetActive(true);
		}
	}

	public int getSoulScore() {
		return souls;
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
