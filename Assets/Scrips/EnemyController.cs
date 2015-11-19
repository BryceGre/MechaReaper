using UnityEngine;
using System.Collections;

public abstract class EnemyController : MonoBehaviour {
	
	public Transform Player = null;
	public float MoveSpeed = 20.0f;
	public float RotateSpeed = 1.0f;
	public int health = 10;

	public bool Fear = false;

	protected float maxMoveSpeed;
	protected float maxRotateSpeed;

	protected int instance = 0;
	public GameObject explosion;
	public GameObject soulPickUp;

	public virtual void Start() {
		maxMoveSpeed = MoveSpeed;
		maxRotateSpeed = RotateSpeed;
	}
	
	public void applyDamage(int damage) {
		health -= damage;
	}

	public void destroyEnemy(){
		Instantiate (explosion, this.transform.position, this.transform.rotation);
		Instantiate (soulPickUp, this.transform.position, this.transform.rotation);
		//Destroy (this.gameObject);
		gameObject.SetActive(false);
		health = 10;
		instance++;
	}

	public int getInstance() {
		return instance;
	}

	public void resetSpeed() {
		MoveSpeed = maxMoveSpeed;
		RotateSpeed = maxRotateSpeed;
	}
}
