using UnityEngine;
using System.Collections;

public class EnemyMechController : EnemyController {
	public int damage = 2;
	public float fireTime = 0.5f;
	private float fireTimer = 0.0f;
	private int muzzleFlashTimer = -1;
	public GameObject muzzleFlash;
	private bool left = false;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			this.destroyEnemy();
		}

		Vector3 moveDir = gameObject.transform.right;
		if (left) 
			moveDir = -moveDir;
		Vector3 toPlayer = Vector3.Normalize(Player.transform.position - gameObject.transform.position);
		float distance = Vector3.Distance (gameObject.transform.position, Player.transform.position);
		if (distance < 28.0f) {
			moveDir = Vector3.Normalize(moveDir - toPlayer);
		} else if (distance > 32.0f) {
			moveDir = Vector3.Normalize(moveDir + toPlayer);
		}
		Vector3 rotation = Vector3.RotateTowards (transform.forward, toPlayer, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);

		//test for asteroids
		RaycastHit hit;
		Ray ray = new Ray (this.gameObject.transform.position, moveDir);
		if (Physics.Raycast (ray, out hit, 10.0f)) {
			GameObject hitObject = hit.collider.gameObject;
			if (hitObject.CompareTag("Debris")) {
				moveDir = -moveDir;
				left = !left;
			}
		}
		
		gameObject.GetComponent<Rigidbody> ().velocity = moveDir * MoveSpeed;
		
		if (muzzleFlashTimer > 0) {
			muzzleFlashTimer --;
			if(muzzleFlashTimer == 0)
			{
				muzzleFlash.SetActive(false);
			}
		}
		
		if (Vector3.Dot (gameObject.transform.forward, toPlayer) > 0.95f) {
			fireTimer += Time.deltaTime;
			while (fireTimer >= fireTime) {
				fireTimer -= fireTime;
				RaycastHit hit2;
				Ray ray2 = new Ray (gameObject.transform.position, gameObject.transform.forward);
				if (Physics.Raycast (ray2, out hit2)) {
					GameObject player = hit2.collider.gameObject;
					if (player.CompareTag ("Player")) {
						muzzleFlash.SetActive(true);
						muzzleFlashTimer = 5;
						if (Random.Range(0, 2) == 0)
							Player.GetComponent<MechController>().applyDamage(damage);
					}
				}
				
			}
		}
	}
}
