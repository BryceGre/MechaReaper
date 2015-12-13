using UnityEngine;
using System.Collections;

public class CataclysmPower : Power {
	private float suckCount;

	private const float moveSpeed = 100.0f;

	public override void Start() {
		base.Start ();
	}
	
	public override void Update() {
		base.Update ();

	}

	void LateUpdate() {
		if (durationCount > 0.0f) {
			GameObject[] souls = GameObject.FindGameObjectsWithTag("Soul");
			foreach (GameObject soul in souls) {
				Vector3 toPlayer = Vector3.Normalize(this.gameObject.transform.position - soul.transform.position);
				soul.GetComponent<Rigidbody> ().velocity = (toPlayer * moveSpeed);
			}
		}
	}
	
	public override bool usePower() {
		if (!base.usePower ())
			return false;
		
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().destroyEnemy();
		}

		return true;
	}
}
