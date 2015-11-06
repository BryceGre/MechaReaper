using UnityEngine;
using System.Collections;

public class ShockwavePower : Power {
	public float Distance = 25.0f;
	public float Duration = 5.0f;
	private float durationCounter = 0.0f;

	public Transform PowerFX;
	private Transform fxInstance = null;

	public override void Start() {
		base.Start ();
	}

	public override void Update() {
		base.Update ();
		if (durationCounter > 0.0f) {
			durationCounter -= Time.deltaTime;
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			if (durationCounter <= 0.0f) {
				foreach (GameObject enemy in enemies) {
					//reset enemy speed to max
					enemy.GetComponent<EnemyController>().resetSpeed();
				}
				durationCounter = 0.0f;
				if (fxInstance != null)
					Destroy(fxInstance.gameObject);
			} else {
				foreach (GameObject enemy in enemies) {
					//slow enemy speed to 0 over 5 seconds
					enemy.GetComponent<EnemyController>().MoveSpeed = durationCounter;
				}
				if (fxInstance != null) {
					fxInstance.gameObject.transform.localScale *= 1.25f;
				}
			}
		}
	}

	public override void usePower() {
		if (this.isOnCooldown()) return;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) {
			if (Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) <= Distance) {
				EnemyController control = enemy.GetComponent<EnemyController>();
				control.MoveSpeed = Duration;
				control.RotateSpeed = 0.0f;
			}
		}

		if (fxInstance != null)
			Destroy (fxInstance.gameObject);
		fxInstance = (Transform)Instantiate(PowerFX, this.gameObject.transform.position, Quaternion.identity);

		durationCounter = Duration;
		startCooldown ();
	}
}
