using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	public Transform Player = null;
	public float MoveSpeed = 20.0f;
	public float RotateSpeed = 1.0f;
	public int health = 10;

	protected int instance = 0;
	public GameObject explosion;
	public GameObject soulPickUp;
	
	public void applyDamage(int damage) {
		health -= damage;
	}

	public void destroyEnemy(){
		Instantiate (explosion, this.transform.position, this.transform.rotation);
		Instantiate (soulPickUp, this.transform.position, this.transform.rotation);
		//Destroy (this.gameObject);
		gameObject.transform.position = Player.transform.position + GameController.randomPointOnSphere(50.0f);
		health = 10;
		instance++;
	}

	public int getInstance() {
		return instance;
	}
}
