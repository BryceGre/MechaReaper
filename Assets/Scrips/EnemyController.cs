using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	public Transform Player = null;
	public float MoveSpeed = 20.0f;
	public float RotateSpeed = 1.0f;
	public int health = 10;
	
	public GameObject explosion;
	
	public void applyDamage(int damage) {
		health -= damage;
	}

	void destroyEnemy(){
		Instantiate (explosion, this.transform.position, this.transform.rotation);
		Destroy (this.gameObject);
	}
}
