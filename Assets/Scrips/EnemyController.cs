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

	void destroyEnemy(){
		Instantiate (explosion, this.transform.position, this.transform.rotation);
		Destroy (this.gameObject);
		Instantiate (soulPickUp, this.transform.position, this.transform.rotation);
	}

	public int getInstance() {
		return instance;
	}
}
