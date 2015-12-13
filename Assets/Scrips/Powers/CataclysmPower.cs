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
		if (suckCount > 0.0f) {
			GameObject[] souls = GameObject.FindGameObjectsWithTag("Soul");
			suckCount -= Time.deltaTime;
			foreach (GameObject soul in souls) {
				Vector3 toPlayer = Vector3.Normalize(this.gameObject.transform.position - soul.transform.position);
				float distance = Vector3.Distance(this.gameObject.transform.position, soul.transform.position);
				if (distance > moveSpeed) {
					soul.transform.Translate(toPlayer * moveSpeed * Time.deltaTime);
				} else {
					soul.transform.Translate(toPlayer * distance * Time.deltaTime);
				}
			}
		}
	}
	
	public override bool usePower() {
		if (!base.usePower ())
			return false;
		suckCount = 5.0f;
		
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().destroyEnemy();
		}

		return true;
	}
}
