using UnityEngine;
using System.Collections;

public class EnemyShipController : EnemyController {
	private bool pass = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Instantiate (explosion, this.transform.position, this.transform.rotation);
			Destroy (this.gameObject);
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

		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
	}

}
