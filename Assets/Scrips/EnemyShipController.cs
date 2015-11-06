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
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Instantiate (explosion, this.transform.position, this.transform.rotation);
			//Destroy (this.gameObject);
			gameObject.transform.position = Player.transform.position + GameController.randomPointOnSphere(50.0f);
			health = 10;
			instance++;
		}

		Vector3 toPlayer = Player.transform.position - gameObject.transform.position;
		float distance = Vector3.Distance (gameObject.transform.position, Player.transform.position);
		if (distance < 10.0f && pass == false) {
			pass = true;
			//jiggle up trajectory a bit
			toPlayer = (Player.transform.position + Random.insideUnitSphere) - gameObject.transform.position;
		} else if (distance > 30.0f && pass == true) {
			pass = false;
		}
		if (pass == true)
			toPlayer = -toPlayer;
		Vector3 rotation = Vector3.RotateTowards (transform.forward, toPlayer, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);

		if (muzzleFlashTimer > 0) {
			muzzleFlashTimer --;
			if(muzzleFlashTimer == 0)
			{
				muzzleFlash.SetActive(false);
			}
		}

		if (pass == false && Vector3.Dot (gameObject.transform.forward, toPlayer) > 0.95f) {
			fireTimer += Time.deltaTime;
			while (fireTimer >= fireTime) {
				fireTimer -= fireTime;
				muzzleFlash.SetActive(true);
				muzzleFlashTimer = 5;
				if (Random.Range(0, 2) == 0)
					Player.GetComponent<MechController>().applyDamage(damage);
			}
		}

		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
	}

}
