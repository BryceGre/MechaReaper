using UnityEngine;
using System.Collections;

public class EnemyShipController : EnemyController {
	private bool pass = false;
	public int damage = 2;
	public float fireTime = 0.5f;
	private float fireTimer = 0.0f;
	private int muzzleFlashTimer = -1;
	public GameObject muzzleFlash;

	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			this.destroyEnemy();
		}

		//test for asteroids
		RaycastHit hit;
		Ray ray = new Ray (this.gameObject.transform.position, this.gameObject.transform.forward);
		if (Physics.Raycast (ray, out hit, 10.0f)) {
			GameObject hitObject = hit.collider.gameObject;
			if (hitObject.CompareTag("Debris")) {
				Vector3 toObject = Vector3.Normalize(hitObject.transform.position - this.gameObject.transform.position);
				Vector3 avoid = Vector3.RotateTowards (this.gameObject.transform.forward, -toObject, RotateSpeed * Time.deltaTime, 0.0f);
				this.gameObject.transform.rotation = Quaternion.LookRotation(avoid);
				this.gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
				return;
			}
		}
			
		Vector3 toPlayer = Vector3.Normalize(Player.transform.position - gameObject.transform.position);
		float distance = Vector3.Distance (gameObject.transform.position, Player.transform.position);
		if (distance < 10.0f && pass == false) {
			pass = true;
			//jiggle up trajectory a bit
			toPlayer = Vector3.Normalize((Player.transform.position + Random.insideUnitSphere) - gameObject.transform.position);
		} else if (distance > 40.0f && pass == true) {
			pass = false;
		}
		if (pass == true || Fear == true)
			toPlayer = -toPlayer;
		Vector3 rotation = Vector3.RotateTowards (transform.forward, toPlayer, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);
		
		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;

		if (muzzleFlashTimer > 0) {
			muzzleFlashTimer --;
			if(muzzleFlashTimer == 0)
			{
				muzzleFlash.SetActive(false);
			}
		}

		if (pass == false && Fear == false && Vector3.Dot (gameObject.transform.forward, toPlayer) > 0.95f) {
			fireTimer += Time.deltaTime;
			while (fireTimer >= fireTime) {
				fireTimer -= fireTime;
				RaycastHit hit2;
				Ray ray2 = new Ray (gameObject.transform.position, gameObject.transform.forward);
				if (Physics.Raycast (ray2, out hit2)) {
					GameObject hitObject = hit2.collider.gameObject;
					if (hitObject.CompareTag ("Player")) {
						muzzleFlash.SetActive(true);
						muzzleFlashTimer = 5;
						if (Random.Range(0, 2) == 0)
							Player.GetComponent<MechController>().applyDamage(damage);
					}
					if(hitObject.CompareTag ("Debris")){
						if (Random.Range(0, 2) == 0)
							hitObject.GetComponent<AsteroidController>().applyDamage(damage);
					}
				}

			}
		}
	}

}
