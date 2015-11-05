using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

	public Transform Target = null;
	public float MoveSpeed = 25.0f;
	public float RotateSpeed = 1.0f;
	public float lifeTime = 30.0f;

	private int targetInstance;
	private float life = 0.0f;

	// Use this for initialization
	void Start () {
		targetInstance = Target.GetComponent<EnemyController> ().getInstance ();
	}
	
	// Update is called once per frame
	void Update () {
		life += Time.deltaTime;
		if (life >= lifeTime) 
			Destroy (this.gameObject);

		if (Target == null) {
			Destroy (this.gameObject);
			return;
		}
		if (targetInstance != Target.GetComponent<EnemyController> ().getInstance ()) {
			Destroy(this.gameObject);
			return;
		}
		Vector3 toTarget = Target.transform.position - gameObject.transform.position;
		Vector3 rotation = Vector3.RotateTowards (transform.forward, toTarget, RotateSpeed * Time.deltaTime, 0.0f);
		gameObject.transform.rotation = Quaternion.LookRotation(rotation);
		
		gameObject.GetComponent<Rigidbody> ().velocity = gameObject.transform.forward * MoveSpeed;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemy in enemies) {
				float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position) / GameController.Unit;
				int damage = Mathf.RoundToInt(-2.0f*Mathf.Pow(distance, 2.0f)+15.0f);
				if (damage > 0)
					enemy.GetComponent<EnemyController>().applyDamage(damage);
			}
			Destroy(gameObject);
		}
	}
}
